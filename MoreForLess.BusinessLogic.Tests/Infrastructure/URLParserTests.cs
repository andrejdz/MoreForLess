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

        [Test]
        public void URLParser_Parse_IdNotExistException()
        {
            URLParser parser = new URLParser();

            Assert.Throws(typeof(ArgumentException), () => parser.Parse("https://catalog.onliner.by/"));
        }

        [Test]
        public void URLParser_Parse_ValidURLInfoReturned()
        {
            URLParser parser = new URLParser();
            var info = parser.Parse("https://catalog.onliner.by/desktoppc/intel/stk1aw32sc");

            Assert.AreEqual("desktoppc/intel/stk1aw32sc", info.Id);
            Assert.AreEqual("Onliner", info.Platform);
            Assert.AreEqual("https://catalog.onliner.by/desktoppc/intel/stk1aw32sc", info.AbsoluteUri);
        }

        [Test]
        public void URLParser_Parse_ValidURLInfoReturnedAliExpress()
        {
            URLParser parser = new URLParser();
            var info = parser.Parse("https://www.aliexpress.com/item/women-dress-summer-201"
                                    + "6-elegant-new-arrivals-runway-sexy-club-v-neck-dress-s"
                                    + "tretch-Black-fold/32737996228.html?spm=2114.search0103.3"
                                    + ".11.13b43d37iWEUQ7&ws_ab_test=searchweb0_0,searchweb201602"
                                    + "_4_10152_10151_10065_10344_10068_10342_10343_10340_10341_100"
                                    + "84_10083_10618_10630_10307_5722316_5711215_10313_10059_10534_"
                                    + "100031_10629_10103_10626_10624_10623_10622_10621_10620_10142-57"
                                    + "22316_10621,searchweb201603_25,ppcSwitch_5&algo_expid=e4877221-47"
                                    + "14-41b3-9d52-5c85e50cf531-1&algo_pvid=e4877221-4714-41b3-9d52-5c85e5"
                                    + "0cf531&transAbTest=ae803_5&priceBeautifyAB=0");

            Assert.AreEqual("item/women-dress-summer-2016-elegant-new-arrivals-runway-sexy-club-"
                            + "v-neck-dress-stretch-Black-fold/32737996228.html", info.Id);
            Assert.AreEqual("AliExpress", info.Platform);
            Assert.AreEqual("https://www.aliexpress.com/item/women-dress-summer-2016-"
                            + "elegant-new-arrivals-runway-sexy-club-v-neck-dress-stretch"
                            + "-Black-fold/32737996228.html", info.AbsoluteUri);
        }

    }
}