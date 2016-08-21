using System;
using System.Security;
using NUnit.Framework;

namespace MailBot.Security
{
    [TestFixture]
    public class EncryptorTest
    {
        [TestCase("password", "key")]
        public void EncryptsAndDecryptsSecuredString(string password, string key)
        {
            for (var i = 0; i < 10; ++i)
            {
                Console.WriteLine( password );

                var securedPassword = new SecureString();
                foreach (var c in password)
                {
                    securedPassword.AppendChar( c );
                }

                var encryptedString = Encryptor.EncryptString( password, key );
                Console.WriteLine( encryptedString );
                var decryptedString = Encryptor.DecryptString( encryptedString, key );
                Console.WriteLine( decryptedString );
                Console.WriteLine();
                Assert.AreEqual( password, decryptedString );
            }
        }

        [Test, TestCase("password", "key")]
        public void EncryptsAndDecryptsCorrectly(string password, string key)
        {
            for (var i = 0; i < 10; ++i)
            {
                Console.WriteLine( password );
                var encryptedString = Encryptor.EncryptString( password, key );
                Console.WriteLine( encryptedString );
                var decryptedString = Encryptor.DecryptString( encryptedString, key );
                Console.WriteLine( decryptedString );
                Console.WriteLine();
                Assert.AreEqual( password, decryptedString );
            }
        }

        [Test, TestCase("password")]
        public void EncryptsAndDecryptsKeylessCorrectly(string password)
        {
            for (var i = 0; i < 10; ++i)
            {
                Console.WriteLine( password );
                var encryptedString = Encryptor.EncryptString( password );
                Console.WriteLine( encryptedString );
                var decryptedString = Encryptor.DecryptString( encryptedString );
                Console.WriteLine( decryptedString );
                Console.WriteLine();
                Assert.AreEqual( password, decryptedString );
            }
        }

        [Test, TestCase("password", "key", "salt")]
        public void EncryptsAndDecryptsSaltedCorrectly(string password, string key, string salt)
        {
            for (var i = 0; i < 10; ++i)
            {
                Console.WriteLine( password );
                var encryptedString = Encryptor.EncryptString( password, key, salt );
                Console.WriteLine( encryptedString );
                var decryptedString = Encryptor.DecryptString( encryptedString, key, salt );
                Console.WriteLine( decryptedString );
                Console.WriteLine();
                Assert.AreEqual( password, decryptedString );
            }
        }

        [Test, TestCase("vasyok choovachock"), TestCase("petrookha")]
        public void Sha(string tohash)
        {
            var hash = Hasher.Encrypt( tohash );
            Console.WriteLine( hash );
            Console.WriteLine( hash.Length );
        }
    }
}