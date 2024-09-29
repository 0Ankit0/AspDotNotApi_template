using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API_TEMPLATE.CustomClass
{
    internal interface ICustomCryptography
    {
        public string Encrypt(string clearText, string key);
        public string Decrypt(string cipherText, string key);
    }

    public class SymmetricCryptography : ICustomCryptography
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

    public class AssymetricCryptographyUsingFileKey : ICustomCryptography
    {
        //Provide a default filepath for storing key pairs
        private string _filePath = "./Data/keyPairs.json";

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
        public string Decrypt(string dataToDecrypt, string publicKey)
        {
            // Retrieve the private key based on the provided public key.
            string privateKey = GetPrivateKeyFromPublicKey(publicKey);

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

        // Generates a pair of RSA keys (public and private) and stores them in a JSON file.
        public void GenerateAndStoreKeys()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;

                string publicKey = Convert.ToBase64String(rsa.ExportCspBlob(false));
                string privateKey = Convert.ToBase64String(rsa.ExportCspBlob(true));

                // Load existing keys from file or create a new dictionary if the file doesn't exist.
                Dictionary<string, string> keyPairs = new Dictionary<string, string>();
                if (File.Exists(_filePath))
                {
                    string existingData = File.ReadAllText(_filePath);
                    keyPairs = JsonSerializer.Deserialize<Dictionary<string, string>>(existingData);
                }

                // Add the new key pair to the dictionary.
                keyPairs[publicKey] = privateKey;

                // Serialize the dictionary back to the file.
                string jsonData = JsonSerializer.Serialize(keyPairs, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, jsonData);
            }
        }

        // Retrieves the private key based on the provided public key.
        public string GetPrivateKeyFromPublicKey(string publicKey)
        {
            if (File.Exists(_filePath))
            {
                string existingData = File.ReadAllText(_filePath);
                var keyPairs = JsonSerializer.Deserialize<Dictionary<string, string>>(existingData);

                if (keyPairs != null && keyPairs.ContainsKey(publicKey))
                {
                    return keyPairs[publicKey];
                }
            }
            throw new Exception("Public key not found.");
        }

    }
    public class AssymetricCryptography : ICustomCryptography
    {
        // Encrypts the provided data using RSA asymmetric cryptography with the specified public key.
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

        // Decrypts the provided data using RSA asymmetric cryptography with the specified private key.
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

        // Generates a pair of RSA keys (public and private) and returns them as base64 strings.
        public (string publicKey, string privateKey) GenerateKeys()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                string publicKey = Convert.ToBase64String(rsa.ExportCspBlob(false));
                string privateKey = Convert.ToBase64String(rsa.ExportCspBlob(true));

                return (publicKey, privateKey);
            }
        }

    }

}
