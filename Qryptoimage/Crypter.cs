using System;  
using System.IO;  
using System.Security.Cryptography;

namespace Qryptoimage
{
    public static class Crypter
    {
        public static string Encrypt(string text, Guid key)  
        {  
            using var aes = Aes.Create();
            aes.Key = key.ToByteArray();//Encoding.UTF8.GetBytes(key);  
            aes.IV = new byte[16];  
  
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (var streamWriter = new StreamWriter(cryptoStream)) 
                streamWriter.Write(text);

            return Convert.ToBase64String(memoryStream.ToArray()); //Encoding.UTF8.GetString(memoryStream.ToArray());
        }  
  
        public static string Decrypt(string text, Guid key)
        {
            var buffer = Convert.FromBase64String(text);//Encoding.UTF8.GetBytes(text);

            using var aes = Aes.Create();
            aes.Key = key.ToByteArray();//Encoding.UTF8.GetBytes(key);  
            aes.IV = new byte[16];  
            
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            
            return streamReader.ReadToEnd();
        }  
    }
}