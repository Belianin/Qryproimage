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
    }
}