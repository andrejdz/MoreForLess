using System;
using NUnit.Framework;
using MoreForLess.BusinessLogic.Infrastructure;

namespace MoreForLess.BusinessLogic.Tests.Infrastructure
{
    [TestFixture]
    public class URLParserTests
    {
        [Test]
        public void URLParser_Parse_URLNullArgumentException()
        {
            URLParser parser = new URLParser();

            Assert.Throws(typeof(ArgumentException), () => parser.Parse(null));
        }

        [Test]
        public void URLParser_Parse_URLEmptyArgumentException()
        {
            URLParser parser = new URLParser();

            Assert.Throws(typeof(ArgumentException), () => parser.Parse(""));
        }

        [Test]
        public void URLParser_Parse_InvalidURLFormatException()
        {
            URLParser parser = new URLParser();

            Assert.Throws(typeof(FormatException), () => parser.Parse("htt://123.c/qwe"));
        }

        [Test]
        public void URLParser_Parse_UnsupportedPlatformException()
        {
            URLParser parser = new URLParser();

            Assert.Throws(typeof(ArgumentException), () => parser.Parse("http://456456.com/qwe"));
        }
    }
}