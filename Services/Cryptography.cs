using System.Text;
using System.Security.Cryptography;

namespace ServiceApp_backend.Services
{
    interface ICryptography
    {
        public string Encrypt(string clearText,string key);
        public string Decrypt(string cipherText,string key);
    }

    public class SymmetricCryptography : ICryptography
    {
        // Method to encrypt a given clear text using AES encryption with PBKDF2 for key derivation.
        public string Encrypt(string clearText, string password = "|3w?.3423df./,;''#asdWDe:';23|")
        {
            // Convert the clear text to bytes using Unicode encoding.
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            // Create an instance of the AES encryption algorithm.
            using (Aes encryptor = Aes.Create())
            {
                // Define the number of iterations for the PBKDF2 hashing algorithm.
                int iterations = 10000;

                // Specify the hash algorithm used for key derivation.
                HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

                // Initialize the PBKDF2 derive bytes object with the specified password, salt, iterations, and hash algorithm.
                // The salt is a predefined byte array here, but in practice, it should be generated randomly and stored securely.
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4e, 0x65, 0x65, 0x64, 0x20, 0x4d, 0x6f, 0x72, 0x65, 0x20, 0x42 }, iterations, hashAlgorithm);

                // Derive the encryption key and initialization vector (IV) from the password using PBKDF2.
                encryptor.Key = pdb.GetBytes(32); // The key size is 256 bits.
                encryptor.IV = pdb.GetBytes(16); // The IV size is 128 bits.

                // Create a memory stream to hold the encrypted data.
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create a crypto stream that writes to the memory stream and uses the specified encryption algorithm.
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Write the clear text bytes to the crypto stream for encryption.
                        cs.Write(clearBytes, 0, clearBytes.Length);

                        // Close the crypto stream, which flushes any buffered data and performs the final encryption.
                        cs.Close();
                    }

                    // Convert the encrypted data back to a Base64 string for easy storage or transmission.
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            // Return the encrypted text as a Base64 string.
            return clearText;
        }



        // Method to decrypt a given cipher text using AES decryption with PBKDF2 for key derivation.
        public string Decrypt(string cipherText, string password = "|3w?.3423df./,;''#asdWDe:';23|")
        {

            // Convert the cipher text from a Base64 string back to bytes.
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Create an instance of the AES decryption algorithm.
            using (Aes encryptor = Aes.Create())
            {
                // Define the number of iterations for the PBKDF2 hashing algorithm.
                int iterations = 10000;

                // Specify the hash algorithm used for key derivation.
                HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

                // Initialize the PBKDF2 derive bytes object with the specified password, salt, iterations, and hash algorithm.
                // The salt is a predefined byte array here, but in practice, it should be generated randomly and stored securely.
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4e, 0x65, 0x65, 0x64, 0x20, 0x4d, 0x6f, 0x72, 0x65, 0x20, 0x42 }, iterations, hashAlgorithm);

                // Derive the encryption key and initialization vector (IV) from the password using PBKDF2.
                encryptor.Key = pdb.GetBytes(32); // The key size is 256 bits.
                encryptor.IV = pdb.GetBytes(16); // The IV size is 128 bits.

                // Create a memory stream to hold the decrypted data.
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create a crypto stream that writes to the memory stream and uses the specified decryption algorithm.
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        // Write the cipher text bytes to the crypto stream for decryption.
                        cs.Write(cipherBytes, 0, cipherBytes.Length);

                        // Close the crypto stream, which flushes any buffered data and performs the final decryption.
                        cs.Close();
                    }

                    // Convert the decrypted data back to a Unicode string.
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            // Return the decrypted text as a Unicode string.
            return cipherText;
        }
    }

    public class AssymetricCryptography : ICryptography
    {
        // Encrypts the provided data using RSA asymmetric cryptography with the specified public key.
        public string Encrypt(string dataToEncrypt, string publicKey)
        {
            // Convert the data to bytes using Unicode encoding.
            byte[] dataBytes = Encoding.Unicode.GetBytes(dataToEncrypt);

            // Variable to hold the encrypted data.
            byte[] encryptedData;

            // Create an instance of the RSA cryptographic service provider with a key size of 2048 bits.
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                // Disable persisting the key in the current security package.
                rsa.PersistKeyInCsp = false;

                // Import the public key from a base64 encoded string.
                rsa.ImportCspBlob(Convert.FromBase64String(publicKey));

                // Encrypt the data bytes using the imported public key.
                encryptedData = rsa.Encrypt(dataBytes, false); // 'false' indicates no OAEP padding.
            }

            // Convert the encrypted data bytes to a base64 string for easy storage or transmission.
            return Convert.ToBase64String(encryptedData);
        }

        // Decrypts the provided data using RSA asymmetric cryptography with the specified private key.
        public string Decrypt(string dataToDecrypt, string privateKey)
        {
            // Convert the encrypted data from a base64 string back to bytes.
            byte[] dataBytes = Convert.FromBase64String(dataToDecrypt);

            // Variable to hold the decrypted data.
            byte[] decryptedData;

            // Create an instance of the RSA cryptographic service provider with a key size of 2048 bits.
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                // Disable persisting the key in the current security package.
                rsa.PersistKeyInCsp = false;

                // Import the private key from a base64 encoded string.
                rsa.ImportCspBlob(Convert.FromBase64String(privateKey));

                // Decrypt the data bytes using the imported private key.
                decryptedData = rsa.Decrypt(dataBytes, false); // 'false' indicates no OAEP padding.
            }

            // Convert the decrypted data bytes to a Unicode string.
            return Encoding.Unicode.GetString(decryptedData);
        }

        // Generates a pair of RSA keys (public and private) and exports them as base64 strings.
        public void GenerateKeys(out string publicKey, out string privateKey)
        {
            // Create an instance of the RSA cryptographic service provider with a key size of 2048 bits.
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                // Disable persisting the key in the current security package.
                rsa.PersistKeyInCsp = false;

                // Export the public key as a base64 string.
                publicKey = Convert.ToBase64String(rsa.ExportCspBlob(false));

                // Export the private key as a base64 string.
                privateKey = Convert.ToBase64String(rsa.ExportCspBlob(true));
            }
        }
    }

}
