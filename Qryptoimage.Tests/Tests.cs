using System;
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
    }
}