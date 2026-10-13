using System.Net;
using System.Net.Sockets;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Creates HTTP handlers that only connect to public internet addresses (SSRF protection).
/// DNS is resolved and validated at connect time, so redirects and DNS rebinding cannot reach
/// loopback, private, link-local (cloud metadata) or otherwise non-public destinations.
/// </summary>
public static class PublicEndpointHandler
{
    public static SocketsHttpHandler Create(bool allowAutoRedirect = true)
        => new()
        {
            AllowAutoRedirect = allowAutoRedirect,
            ConnectCallback = ConnectAsync
        };

    public static bool IsPublicAddress(IPAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (address.IsIPv4MappedToIPv6)
        {
            address = address.MapToIPv4();
        }

        if (IPAddress.IsLoopback(address) || address.Equals(IPAddress.Any) || address.Equals(IPAddress.IPv6Any))
        {
            return false;
        }

        var bytes = address.GetAddressBytes();

        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            return !(bytes[0] == 0                                          // 0.0.0.0/8
                || bytes[0] == 10                                           // 10.0.0.0/8
                || (bytes[0] == 100 && bytes[1] is >= 64 and <= 127)        // 100.64.0.0/10 (CGNAT)
                || (bytes[0] == 169 && bytes[1] == 254)                     // 169.254.0.0/16 (link-local, cloud metadata)
                || (bytes[0] == 172 && bytes[1] is >= 16 and <= 31)         // 172.16.0.0/12
                || (bytes[0] == 192 && bytes[1] == 168)                     // 192.168.0.0/16
                || (bytes[0] == 192 && bytes[1] == 0 && bytes[2] == 0)      // 192.0.0.0/24 (IETF protocol assignments)
                || (bytes[0] == 198 && bytes[1] is 18 or 19)                // 198.18.0.0/15 (benchmarking)
                || bytes[0] >= 224);                                        // multicast, reserved, broadcast
        }

        if (address.AddressFamily == AddressFamily.InterNetworkV6)
        {
            return !(address.IsIPv6LinkLocal
                || address.IsIPv6SiteLocal
                || address.IsIPv6Multicast
                || address.IsIPv6UniqueLocal);
        }

        return false;
    }

    private static async ValueTask<Stream> ConnectAsync(SocketsHttpConnectionContext context, CancellationToken cancellationToken)
    {
        var host = context.DnsEndPoint.Host;

        var addresses = IPAddress.TryParse(host, out var literal)
            ? [literal]
            : await Dns.GetHostAddressesAsync(host, cancellationToken);

        var publicAddresses = addresses.Where(IsPublicAddress).ToArray();
        if (publicAddresses.Length == 0)
        {
            throw new HttpRequestException($"Blocked connection to '{host}': it does not resolve to a public address.");
        }

#pragma warning disable CA2000 // Ownership of the socket transfers to the returned NetworkStream; it is disposed on failure below
        var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
#pragma warning restore CA2000
        try
        {
            socket.NoDelay = true;
            await socket.ConnectAsync(publicAddresses, context.DnsEndPoint.Port, cancellationToken);
            return new NetworkStream(socket, ownsSocket: true);
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }
}
