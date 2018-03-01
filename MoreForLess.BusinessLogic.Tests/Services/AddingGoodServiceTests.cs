using System;
using System.Threading.Tasks;
using Moq;
using NLog;
using NUnit.Framework;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services;
using MoreForLess.BusinessLogic.Services.Interfaces;

namespace MoreForLess.BusinessLogic.Tests.Services
{
    [TestFixture]
    public class AddingGoodServiceTests
    {
        private readonly Mock<IGoodService> _goodService = new Mock<IGoodService>();
        private readonly Mock<IGoodDomainModelCreatorService> _goodDomainModelCreatorService =
            new Mock<IGoodDomainModelCreatorService>();
        private readonly Mock<ILogger> _logger = new Mock<ILogger>();

        private readonly GoodDomainModel _goodDomainModel = new GoodDomainModel()
        {
            Name = "Mi A1",
            Price = 100M,
            LinkOnProduct = "https://catalog.onliner.by/mobile/xiaomi/mia1b",
            LinkOnPicture = "https://content2.onliner.by/catalog/device/header/6289471a631ea937daf914faeb9c5fcc.jpeg",
            IdGoodOnShop = "12qwe12",
            ShopName = "Onliner",
            CurrencyName = "BYN",
            Id = 1
        };

        [OneTimeSetUp]
        public void Initialize()
        {
            this._goodDomainModelCreatorService.Setup(g => g.CreateGoodDomainModelFromUrlAsync(It.IsAny<string>()))
                .ReturnsAsync(this._goodDomainModel);

            this._goodService.Setup(g => g.CreateAsync(It.IsAny<GoodDomainModel>()))
                .ReturnsAsync(this._goodDomainModel);
        }

        [Test]
        public async Task AddingGoodService_ProcessAsync_ValidUrl()
        {
            // Act
            var actual = await new AddingGoodService(
                    this._goodDomainModelCreatorService.Object,
                    this._goodService.Object)
                .ProcessAsync("emptyUrl");

            // Assert
            Assert.That(actual.Id, Is.EqualTo(this._goodDomainModel.Id));
            Assert.That(actual.Name, Is.EqualTo(this._goodDomainModel.Name));
            Assert.That(actual.Price, Is.EqualTo(this._goodDomainModel.Price));
            Assert.That(actual.LinkOnProduct, Is.EqualTo(this._goodDomainModel.LinkOnProduct));
            Assert.That(actual.LinkOnPicture, Is.EqualTo(this._goodDomainModel.LinkOnPicture));
            Assert.That(actual.IdGoodOnShop, Is.EqualTo(this._goodDomainModel.IdGoodOnShop));
            Assert.That(actual.ShopName, Is.EqualTo(this._goodDomainModel.ShopName));
            Assert.That(actual.CurrencyName, Is.EqualTo(this._goodDomainModel.CurrencyName));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void AddingGoodService_ProcessAsync_UrlNullEmptyWhitespacesArgumentException(string url)
        {
            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await new AddingGoodService(
                    this._goodDomainModelCreatorService.Object,
                    this._goodService.Object)
                .ProcessAsync(url));
        }
    }
}