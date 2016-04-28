using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace WsFederationPlugin.EntityFramework.IntegrationTests.Certificates
{
    internal static class Cert
    {
        public static X509Certificate2 LoadEncryptingCertificate()
        {
            var assembly = typeof(Cert).Assembly;
            using (var stream = assembly.GetManifestResourceStream(
                "WsFederationPlugin.EntityFramework.IntegrationTests.Certificates.EncryptionTest.cer"))
            {
                return new X509Certificate2(ReadStream(stream));
            }
        }

        public static X509Certificate2 LoadDecryptingCertificate()
        {
            var assembly = typeof(Cert).Assembly;
            using (var stream = assembly.GetManifestResourceStream(
                "WsFederationPlugin.EntityFramework.IntegrationTests.Certificates.EncryptionTest.pfx"))
            {
                return new X509Certificate2(ReadStream(stream), "p4ssw0rd");
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