using System;
using Moq;
using NUnit.Framework;
using MoreForLess.BusinessLogic.Adapters;
using MoreForLess.BusinessLogic.Infrastructure;
using MoreForLess.BusinessLogic.Services;
using MoreForLess.BusinessLogic.Services.Interfaces;

namespace MoreForLess.BusinessLogic.Tests.Infrastructure
{
    [TestFixture]
    public class AdapterResolverTests
    {
        private readonly Mock<ISignedRequestCreatorService<SignedRequestAmazonCreatorService>> _signedRequestCreatorService =
                new Mock<ISignedRequestCreatorService<SignedRequestAmazonCreatorService>>();

        [Test]
        public void AdapterResolver_ResolveTest_ValidPlatformParserParsingAdapter()
        {
            // Act
            var actual = new AdapterResolver(this._signedRequestCreatorService.Object).Resolve(_platform);

            // Assert
            Assert.That(actual, Is.TypeOf<OnlinerAdapter>());
        }

        [Test]
        public void AdapterResolver_ResolveTest_ValidPlatformAliExpress()
        {
            // Act
            var actual = new AdapterResolver(this._signedRequestCreatorService.Object).Resolve("AliExpress");

            // Assert
            Assert.That(actual, Is.TypeOf<AliExpressAdapter>());
        }


        [Test]
        public void AdapterResolver_Resolve_PlatformIsNotSupportedArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new AdapterResolver(this._signedRequestCreatorService.Object).Resolve("Lamoda"));
        }

        // Arrange
        private const string _platform = "Onliner";
    }
}