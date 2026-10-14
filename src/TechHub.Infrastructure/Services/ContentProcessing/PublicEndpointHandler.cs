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
    private static readonly IPNetwork[] _nonPublicIpv4Networks =
    [
        IPNetwork.Parse("0.0.0.0/8"),
        IPNetwork.Parse("10.0.0.0/8"),
        IPNetwork.Parse("100.64.0.0/10"),
        IPNetwork.Parse("127.0.0.0/8"),
        IPNetwork.Parse("169.254.0.0/16"),
        IPNetwork.Parse("172.16.0.0/12"),
        IPNetwork.Parse("192.0.0.0/24"),
        IPNetwork.Parse("192.0.2.0/24"),
        IPNetwork.Parse("192.88.99.0/24"),
        IPNetwork.Parse("192.168.0.0/16"),
        IPNetwork.Parse("198.18.0.0/15"),
        IPNetwork.Parse("198.51.100.0/24"),
        IPNetwork.Parse("203.0.113.0/24"),
        IPNetwork.Parse("224.0.0.0/4"),
        IPNetwork.Parse("240.0.0.0/4")
    ];

    private static readonly IPNetwork _ipv6GlobalUnicastNetwork = IPNetwork.Parse("2000::/3");

    private static readonly IPNetwork[] _nonPublicIpv6Networks =
    [
        IPNetwork.Parse("2001::/23"),
        IPNetwork.Parse("2001:db8::/32"),
        IPNetwork.Parse("2002::/16"),
        IPNetwork.Parse("3fff::/20"),
        IPNetwork.Parse("5f00::/16")
    ];

    public static SocketsHttpHandler Create(bool allowAutoRedirect = true)
        => new()
        {
            AllowAutoRedirect = allowAutoRedirect,
            // A proxy would be the connect target, so the validated endpoint would no longer be the real destination
            UseProxy = false,
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

        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            return !_nonPublicIpv4Networks.Any(network => network.Contains(address));
        }

        if (address.AddressFamily == AddressFamily.InterNetworkV6)
        {
            return _ipv6GlobalUnicastNetwork.Contains(address)
                && !_nonPublicIpv6Networks.Any(network => network.Contains(address));
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
