---
external_url: https://devblogs.microsoft.com/dotnet/post-quantum-cryptography-in-dotnet/
title: 'Post-Quantum Cryptography in .NET: New Algorithms and Design Principles'
author: Jeremy Barton
feed_name: Microsoft .NET Blog
date: 2025-11-18 16:00:00 +00:00
tags:
- .NET
- .NET 10
- CertificateRequest
- CmsSigner
- CNG
- Composite ML DSA
- Cryptography
- Cryptography API
- Experimental Features
- Key Encapsulation
- Microsoft.Bcl.Cryptography
- ML DSA
- ML KEM
- NIST FIPS 203
- NIST FIPS 204
- OpenSSL 3.5
- Post Quantum Cryptography
- PQC
- Quantum
- Signature Algorithms
- SLH DSA
- System.Security.Cryptography
- TLS 1.3
- X509Certificate2
section_names:
- coding
- security
primary_section: coding
---
Jeremy Barton walks through the integration of post-quantum cryptography (PQC) algorithms in .NET 10, explaining new classes, technical challenges, and practical usage for developers.<!--excerpt_end-->

# Post-Quantum Cryptography in .NET

**Author:** Jeremy Barton

With the release of .NET 10, Microsoft has introduced support for several Post-Quantum Cryptography (PQC) algorithms, marking a significant shift in the way .NET applications can be secured against the threats posed by future quantum computers.

## Why PQC?

Current asymmetric algorithms like RSA and ECC could be broken by sufficiently powerful quantum computers. PQC aims to provide cryptographic algorithms that withstand such advances, ensuring long-term security—especially in scenarios where data can be harvested now and decrypted later when quantum computers arrive.

## Algorithms Added in .NET 10

Four major algorithms have been added:

| Algorithm              | Kind                | Specification    | .NET Class          |
|------------------------|---------------------|------------------|---------------------|
| ML-KEM                 | Key Encapsulation   | NIST FIPS 203    | MLKem               |
| ML-DSA                 | Signature           | NIST FIPS 204    | MLDsa               |
| SLH-DSA                | Signature           | NIST FIPS 205    | SlhDsa              |
| Composite ML-DSA       | Signature           | IETF Draft       | CompositeMLDsa      |

- **ML-DSA, SLH-DSA, Composite ML-DSA** are replacements for RSA and EC-DSA signatures.
- **ML-KEM** replaces key transport (RSA) and key agreement (EC-DH).

## Integrating PQC into .NET

The adoption of PQC led to rethinking the core cryptography types. Existing base classes like `AsymmetricAlgorithm` didn’t align with the new algorithms, especially regarding key size and curve specifics. The new design implements algorithm-specific classes derived from `object`, with a focus on minimization of errors and clear separation of concerns:

- Key/keypair is directly represented by an instance.
- Disposal is strictly enforced.
- No forced common base for algorithms with fundamental differences.
- Template Method Pattern centralizes argument validation and flow.
- Spans are heavily used for performance and fixed-size requirements.

### Example: MLDsa API Highlights

```csharp
public abstract partial class MLDsa : IDisposable {
  public static bool IsSupported { get; }
  public MLDsaAlgorithm Algorithm { get; }
  public static MLDsa GenerateKey(MLDsaAlgorithm algorithm);
  public static MLDsa ImportMLDsaPublicKey(MLDsaAlgorithm algorithm, ReadOnlySpan<byte> source);
  public void SignData(ReadOnlySpan<byte> data, Span<byte> destination, ReadOnlySpan<byte> context = default);
  // ...Other methods omitted
}
```

## Practical Usage and Getting Started

- **Requirements:** .NET 10, OS support for algorithms
  - Linux: OpenSSL 3.5+
  - Windows: Windows 11 (Patch Tuesday updates)
  - For .NET Standard 2.0, reference Microsoft.Bcl.Cryptography v10.0+
- **Detection:** Use `IsSupported` properties to verify platform compatibility.

### Sample ML-KEM Usage

```csharp
using System.Security.Cryptography;
if (!MLKem.IsSupported) {
  Console.WriteLine("ML-KEM isn't supported :(");
  return;
}
MLKemAlgorithm alg = MLKemAlgorithm.MLKem768;
using (MLKem privateKey = MLKem.GenerateKey(alg))
using (MLKem publicKey = MLKem.ImportEncapsulationKey(alg, privateKey.ExportEncapsulationKey())) {
  publicKey.Encapsulate(out byte[] ciphertext, out byte[] sharedSecret1);
  byte[] sharedSecret2 = privateKey.Decapsulate(ciphertext);
  // Compare shared secrets
}
```

## Experimental Features

Some algorithms/classes are marked `[Experimental]` because of evolving specs or incomplete OS support:

- `SlhDsa` and `CompositeMLDsa` classes are experimental.
- Select methods on `MLKem` and `MLDsa` are experimental, such as PEM imports/exports and signature variants.

## Where PQC Algorithms are Used

- Certificate generation (`CertificateRequest`)
- Signed CMS (`CmsSigner`)
- COSE signatures (with new `CoseKey` types)
- X509Certificate2 objects
- TLS (with ML-DSA/SLH-DSA, requires TLS 1.3+ and OS support)

If you run into surprises, feedback can be submitted via GitHub issues ([dotnet/runtime](https://github.com/dotnet/runtime/issues)).

## Special Thanks

The implementation benefited from contributions by GitHub Security Services, OpenSSL, several Linux distros, Windows Cryptography, and IETF LAMPS-WG.

## Links and References

- [Post-Quantum Cryptography in .NET Blog Post](https://devblogs.microsoft.com/dotnet/post-quantum-cryptography-in-dotnet/)
- [NIST FIPS 203](https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.203.pdf)
- [NIST FIPS 204](https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.204.pdf)
- [NIST FIPS 205](https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.205.pdf)
- [Microsoft.Bcl.Cryptography NuGet](https://www.nuget.org/packages/Microsoft.Bcl.Cryptography/)
- [dotnet/runtime GitHub Issues](https://github.com/dotnet/runtime/issues)

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/post-quantum-cryptography-in-dotnet/)
