using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Generator
{
    internal class Cryptographic
    {

        AesCryptoServiceProvider aes;
        
        public Cryptographic(byte[] Key)
        {
            aes = new AesCryptoServiceProvider();

            aes.BlockSize= 128;
            aes.KeySize = 256;
            aes.GenerateIV();
            aes.Key= Key;
            aes.Mode= CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
        }
        public Cryptographic(byte[] IV, byte[] Key)
        {
            aes = new AesCryptoServiceProvider();

            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV= IV;
            aes.Key = Key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
        }

        public string Encrypt(string clearText)
        {
            ICryptoTransform transform = aes.CreateEncryptor();

            byte[] encryptedByte = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(clearText),0, clearText.Length);

            string output = Convert.ToBase64String(encryptedByte);

            return output;
        }

        public string Decrypt(string cipherText)
        {
            ICryptoTransform transform = aes.CreateDecryptor();
            byte[] encryptedByte = Convert.FromBase64String(cipherText);

            byte[] decryptedByte = transform.TransformFinalBlock(encryptedByte, 0, encryptedByte.Length);

            string output = ASCIIEncoding.ASCII.GetString(decryptedByte);
            return output;
        }

        public byte[] getIV()
        {
            return aes.IV;
        }

    }
}
