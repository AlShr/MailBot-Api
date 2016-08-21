using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MailBotDAL.Security;

namespace SharedTypesTests
{
    [TestClass]
    public class EncryptorTests
    {
        [TestMethod]
        public void EncryptTest()
        {
            var cases = new List<string>
            {
                "",
                "12345",
                "12345",
                "23456",
                "34567",
                "vasya",
                "bqa5vq"

            };

            foreach (var @case in cases)
            {
                Console.WriteLine(Encryptor.Encrypt(@case));
            }
        }
    }
}