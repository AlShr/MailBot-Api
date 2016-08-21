using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedTypes.Validation;

namespace SharedTypesTests
{
    [TestClass]
    public class UserInputValidatorTests
    {
        [TestMethod]
        public void AssertUsernameRuleTests()
        {
            var testCases = new Dictionary<string, bool>
            {
                { "lol", false }, //username should be at least 6 symbols long
                { "loller", true }, //ok
                { "3xp3ct3d", true }, //also ok, digits are allowed
                { "SyMbOlS_.123", true }, //also ok, underscore and dots are allowed
                { "*unexpectedSymbol", false } //not ok, nothing except letters, digits, underscore and a dot is allowed
            };

            foreach (var testCase in testCases)
            {
                TryTestCase( UserInputValidator.AssertUsernameRule,
                    testCase.Key,
                    testCase.Value );
            }
        }

        [TestMethod]
        public void AssertPasswordRuleTests()
        {
            var testCases = new Dictionary<string, bool>
            {
                { "lol", false }, //password should be at least 6 symbols long
                { "abcdef", false }, //no digits
                { "123456", false }, //no letters
                { "a1b2c3", true } //ok
            };

            foreach (var testCase in testCases)
            {
                TryTestCase( UserInputValidator.AssertPasswordRule,
                    testCase.Key,
                    testCase.Value );
            }
        }

        private static void TryTestCase(Action<string> action, string testCase, bool expectedResult)
        {
            bool result;
            try
            {
                action( testCase );
                result = true;
            }
            catch
            {
                result = false;
            }
            Assert.AreEqual( result, expectedResult,
                string.Format( "Drops on {0} case. Please attend.", testCase ) );
        }
    }
}