using System.Security.Cryptography;
using System.Text;

namespace NurseryMart.Utility
{
    public static class Helper
    {
        public static byte[] GenerateSalt()
        {
            // Generate a random salt of length 16 bytes (128 bits)
            byte[] salt = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        public static string HashAnything(string anything, byte[] salt)
        {
            // Combine password and salt into byte array
            byte[] passwordBytes = Encoding.UTF8.GetBytes(anything);
            byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

            // Hash combined byte array with SHA256 algorithm
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }
        public static string GenerateOtp(int length)
        {
            if (length < 1 || length > 9)
            {
                throw new ArgumentException("Length must be between 1 and 9", nameof(length));
            }
            byte[] buffer = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                int max = (int)Math.Pow(10, length) - 1;
                int generatedNumber;
                rng.GetBytes(buffer);
                int number = BitConverter.ToInt32(buffer, 0);
                generatedNumber = Math.Abs(number % max);
                //do
                //{
                //    rng.GetBytes(buffer);
                //    int number = BitConverter.ToInt32(buffer, 0);
                //    generatedNumber = Math.Abs(number % max);
                //} while (generatedNumber % 10 == 0);

                return generatedNumber.ToString($"D{length}");
            }
        }
        public static (byte[] Key, byte[] IV) DeriveKeyAndIV(string password, byte[] salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] key = deriveBytes.GetBytes(32); // AES-256 key size
                byte[] iv = deriveBytes.GetBytes(16);  // AES block size
                return (key, iv);
            }
        }
        public static void EncryptFile(Stream inputFileStream, Stream outputFileStream, byte[] key, byte[] iv)
        {
            int KeySize = 256;
            int BlockSize = 128;
            if (key == null || key.Length != 32) throw new ArgumentException("Key must be 32 bytes (256 bits).");
            if (iv == null || iv.Length != 16) throw new ArgumentException("IV must be 16 bytes (128 bits).");

            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(), CryptoStreamMode.Write, true))
                {
                    inputFileStream.CopyTo(cryptoStream);
                }
            }
        }
        public static void DecryptFile(Stream inputFileStream, Stream outputFileStream, byte[] key, byte[] iv)
        {
            int KeySize = 256;
            int BlockSize = 128;
            if (key == null || key.Length != 32) throw new ArgumentException("Key must be 32 bytes (256 bits).");
            if (iv == null || iv.Length != 16) throw new ArgumentException("IV must be 16 bytes (128 bits).");

            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var cryptoStream = new CryptoStream(inputFileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cryptoStream.CopyTo(outputFileStream);
                }
            }
        }
    }
}
