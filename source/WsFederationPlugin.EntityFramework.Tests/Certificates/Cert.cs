using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace WsFederationPlugin.EntityFramework.Tests.Certificates
{
    internal static class Cert
    {
        public static X509Certificate2 LoadEncryptingCertificate()
        {
            var assembly = typeof(Cert).Assembly;
            using (var stream = assembly.GetManifestResourceStream(
                "WsFederationPlugin.EntityFramework.Tests.Certificates.EncryptionTest.cer"))
            {
                return new X509Certificate2(ReadStream(stream));
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16*1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}