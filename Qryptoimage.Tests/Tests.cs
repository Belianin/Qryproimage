using System;
using System.Drawing;
using NUnit.Framework;

namespace Qryptoimage.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Encrypt_and_decrypt_text()
        {
            var text = "Some text that i want to pass through";
            var key = Guid.NewGuid();

            var encrypted = Crypter.Encrypt(text, key);
            var decrypted = Crypter.Decrypt(encrypted, key);
            
            Assert.AreEqual(text, decrypted);
        }
        
        [Test]
        public void Encrypt_and_decrypt_non_ascii_text()
        {
            var text = "Привет мир!😀";
            var key = Guid.NewGuid();

            var encrypted = Crypter.Encrypt(text, key);
            var decrypted = Crypter.Decrypt(encrypted, key);
            
            Assert.AreEqual(text, decrypted);
        }
        
        [Test]
        public void Encode_and_decode_text_to_bitmap()
        {
            var text = "Some text that i want to pass through";
            var bitmap = new Bitmap(50, 50);
            
            LSB.Encode(bitmap, text);
            var decoded = LSB.Decode(bitmap);
            
            Assert.AreEqual(text, decoded);
        }

        [Test]
        public void Set_and_check_watermark()
        {
            var bitmap = new Bitmap(50, 50);
            
            LSB.SetWatermark(bitmap);
            Assert.True(LSB.CheckWatermark(bitmap));
        }

        [Test]
        public void All_together_now()
        {
            var text = "Some text that i want to pass through";
            var key = Guid.NewGuid();
            var bitmap = new Bitmap(50, 50);

            var encrypted = Crypter.Encrypt(text, key);
            
            LSB.Encode(bitmap, encrypted);
            var decoded = LSB.Decode(bitmap);
            var decrypted = Crypter.Decrypt(decoded, key);
            
            Assert.AreEqual(text, decrypted);
        }
    }
}