using System.Text;
using System.Security.Cryptography;

namespace ServiceApp_backend.Services
{
    interface ICryptography
    {
        public string Encrypt(string clearText,string password);
        public string Decrypt(string cipherText,string password);
    }

    public class SymmetricCryptography : ICryptography
    {
        public string Encrypt(string clearText, string password="|3w?.3423df./,;''#asdWDe:';23|")
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                // Specify the hash algorithm and the number of iterations explicitly
                int iterations = 10000;
                HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4e, 0x65, 0x65, 0x64, 0x20, 0x4d, 0x6f, 0x72, 0x65, 0x20, 0x42 }, iterations, hashAlgorithm);

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }


        public string Decrypt(string cipherText, string password= "|3w?.3423df./,;''#asdWDe:';23|")
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                // Specify the hash algorithm and the number of iterations explicitly
                int iterations = 10000;
                HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4e, 0x65, 0x65, 0x64, 0x20, 0x4d, 0x6f, 0x72, 0x65, 0x20, 0x42 }, iterations, hashAlgorithm);

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }

    public class AssymetricCryptography : ICryptography
    {
        public string Encrypt(string dataToEncrypt, string publicKey)
        {
            byte[] dataBytes = Encoding.Unicode.GetBytes(dataToEncrypt);
            byte[] encryptedData;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportCspBlob(Convert.FromBase64String(publicKey));

                encryptedData = rsa.Encrypt(dataBytes, false);
            }

            return Convert.ToBase64String(encryptedData);
        }


        public string Decrypt(string dataToDecrypt, string privateKey)
        {
            byte[] dataBytes = Convert.FromBase64String(dataToDecrypt);
            byte[] decryptedData;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportCspBlob(Convert.FromBase64String(privateKey));

                decryptedData = rsa.Decrypt(dataBytes, false);
            }

            return Encoding.Unicode.GetString(decryptedData);
        }

        //GenerateKeys(out string publicKey, out string privateKey);

        public void GenerateKeys(out string publicKey, out string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                publicKey = Convert.ToBase64String(rsa.ExportCspBlob(false)); // Export public key
                privateKey = Convert.ToBase64String(rsa.ExportCspBlob(true)); // Export private key
            }
        }
    }

}
