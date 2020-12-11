using System;
using System.IO;
using System.Security.Cryptography;

namespace Core.Crypt
{
    internal class Cipher : IDisposable
    {
        private readonly Aes _aes;

        public Cipher(byte[] key)
        {
            _aes = Aes.Create();
            _aes.Mode = CipherMode.CBC;
            _aes.Padding = PaddingMode.None;
            _aes.Key = key;
            _aes.IV = new byte[16];
        }

        private static byte[] PerformCryptography(ICryptoTransform cryptoTransform, byte[] input, int inputOffset,
            int inputLength)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(input, inputOffset, inputLength);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Decrypts data in a single-part operation, or finishes a multiple-part operation.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal byte[] Decrypt(byte[] input)
        {
            return Decrypt(input, 0, input.Length);
        }

        /// <summary>
        /// Decrypts data in a single-part operation, or finishes a multiple-part operation.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputOffset"></param>
        /// <param name="inputLength"></param>
        /// <returns></returns>
        internal byte[] Decrypt(byte[] input, int inputOffset, int inputLength)
        {
            using (var decryptor = _aes.CreateDecryptor(_aes.Key, _aes.IV))
            {
                return PerformCryptography(decryptor, input, inputOffset, inputLength);
            }
        }

        /// <summary>
        /// Encrypts data in a single-part operation, or finishes a multiple-part operation.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputOffset"></param>
        /// <param name="inputLength"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] input, int inputOffset, int inputLength)
        {
            using (var encryptor = _aes.CreateEncryptor(_aes.Key, _aes.IV))
            {
                return PerformCryptography(encryptor, input, inputOffset, inputLength);
            }
        }

        public void Dispose()
        {
            _aes?.Dispose();
        }
    }
}