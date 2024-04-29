using ConsoleApp3;
using NUnit.Framework;

namespace TestProject1
{
    
    public class Tests 
    {
        private Util claseUtil;
        public Tests()
        {
            claseUtil = new Util();
        }

        [Test]
        public void TestEsPalindromo()
        {
            var response = claseUtil.RevisionPalindromo("abba");
            Assert.True(response);
        }
        [Test]
        public void TestNoEsPalindromo()
        {
            var response = claseUtil.RevisionPalindromo("abca");
            Assert.False(response);
        }
    }
}