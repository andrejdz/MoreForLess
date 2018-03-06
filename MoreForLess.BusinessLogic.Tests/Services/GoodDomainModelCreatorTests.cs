//using System;
//using System.Net.Http;
//using System.Threading.Tasks;
//using AutoMapper;
//using Moq;
//using NUnit.Framework;
//using MoreForLess.BusinessLogic.Adapters.Interfaces;
//using MoreForLess.BusinessLogic.Infrastructure.Interfaces;
//using MoreForLess.BusinessLogic.Models;
//using MoreForLess.BusinessLogic.Services;
//using MoreForLess.BusinessLogic.Services.Interfaces;

//namespace MoreForLess.BusinessLogic.Tests.Services
//{
//    [TestFixture]
//    public class GoodDomainModelCreatorTests
//    {
//        private readonly Mock<IGoodService> _goodService = new Mock<IGoodService>();
//        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
//        private readonly Mock<IStoreAdapter> _parsingAdapter = new Mock<IStoreAdapter>();
//        private readonly Mock<IAdapterResolver> _adapterResolver = new Mock<IAdapterResolver>();

//        private readonly GoodDomainModel _goodDomainModel = new GoodDomainModel()
//        {
//            Name = "Mi A1",
//            Price = 100M,
//            LinkOnProduct = "https://catalog.onliner.by/mobile/xiaomi/mia1b",
//            LinkOnPicture = "https://content2.onliner.by/catalog/device/header/6289471a631ea937daf914faeb9c5fcc.jpeg",
//            IdGoodOnShop = "12qwe12",
//            ShopName = "Onliner",
//            CurrencyName = "BYN",
//            Id = 1
//        };

//        private readonly ItemInfo _itemInfo = new ItemInfo()
//        {
//            Name = "Name",
//            Price = 100M,
//            Currency = "Currency",
//            ImageURL = "ImageUrl"
//        };

//        private readonly URLInfo _urlInfo = new URLInfo()
//        {
//            Id = "Id",
//            Platform = "Platform",
//            AbsoluteUri = "AbsoluteUri"
//        };

//        [OneTimeSetUp]
//        public void Initialize()
//        {
//            this._goodService.Setup(gs => gs.IsItemExistAsync(It.IsAny<string>()))
//                .ReturnsAsync(false);

//            this._parsingAdapter.Setup(a => a.GetItemInfoByURLAsync(It.IsAny<URLInfo>()))
//                .ReturnsAsync(this._itemInfo);

//            this._adapterResolver.Setup(a => a.Resolve(It.IsAny<string>()))
//                .Returns(this._parsingAdapter.Object);

//            this._mapper.Setup(m => m.Map<GoodDomainModel>(It.IsAny<URLInfo>()))
//                .Returns(new GoodDomainModel());

//            this._mapper.Setup(m => m.Map(It.IsAny<ItemInfo>(), It.IsAny<GoodDomainModel>()))
//                .Returns<ItemInfo, GoodDomainModel>((ii, gdm) => new GoodDomainModel()
//                {
//                    Name = "Mi A1",
//                    Price = 100M,
//                    LinkOnProduct = "https://catalog.onliner.by/mobile/xiaomi/mia1b",
//                    LinkOnPicture = "https://content2.onliner.by/catalog/device/header/6289471a631ea937daf914faeb9c5fcc.jpeg",
//                    IdGoodOnShop = "12qwe12",
//                    ShopName = "Onliner",
//                    CurrencyName = "BYN",
//                    Id = 1
//                });
//        }

//        [Test]
//        public async Task GoodDomainModelCreator_CreateGoodDomainModelFromUrlAsync_ValidArgumentsGoodDomainModel()
//        {
//            // Act
//            var actual = await new AddUpdateGoodsService(
//                    this._goodService.Object,
//                    this._mapper.Object,
//                    this._urlParser.Object,
//                    this._adapterResolver.Object)
//                .CreateGoodDomainModelFromUrlAsync("empryString");

//            // Assert
//            Assert.That(actual.Id, Is.EqualTo(this._goodDomainModel.Id));
//            Assert.That(actual.Name, Is.EqualTo(this._goodDomainModel.Name));
//            Assert.That(actual.Price, Is.EqualTo(this._goodDomainModel.Price));
//            Assert.That(actual.LinkOnProduct, Is.EqualTo(this._goodDomainModel.LinkOnProduct));
//            Assert.That(actual.LinkOnPicture, Is.EqualTo(this._goodDomainModel.LinkOnPicture));
//            Assert.That(actual.IdGoodOnShop, Is.EqualTo(this._goodDomainModel.IdGoodOnShop));
//            Assert.That(actual.ShopName, Is.EqualTo(this._goodDomainModel.ShopName));
//            Assert.That(actual.CurrencyName, Is.EqualTo(this._goodDomainModel.CurrencyName));
//        }

//        [Test]
//        public void GoodDomainModelCreator_CreateGoodDomainModelFromUrlAsync_GoodAlreadyExistException()
//        {
//            // Arrange
//            Mock<IGoodService> goodService = new Mock<IGoodService>();
//            goodService.Setup(gs => gs.IsItemExistAsync(It.IsAny<string>()))
//                .ReturnsAsync(true);

//            // Assert
//            Assert.ThrowsAsync<ArgumentException>(async () => await
//            new AddUpdateGoodsService(
//                goodService.Object,
//                this._mapper.Object,
//                this._urlParser.Object,
//                this._adapterResolver.Object)
//                .CreateGoodDomainModelFromUrlAsync("It doesn't matter."));
//        }

//        [Test]
//        public void GoodDomainModelCreator_CreateGoodDomainModelFromUrlAsync_AdapterPlatformUnsupportedArgumentException()
//        {
//            // Arrange
//            Mock<IAdapterResolver> adapterResolver = new Mock<IAdapterResolver>();
//            adapterResolver.Setup(ar => ar.Resolve(It.IsAny<string>()))
//                .Throws<ArgumentException>();

//            // Assert
//            Assert.ThrowsAsync<ArgumentException>(async () => await
//            new AddUpdateGoodsService(
//                    this._goodService.Object,
//                    this._mapper.Object,
//                    this._urlParser.Object,
//                adapterResolver.Object)
//                .CreateGoodDomainModelFromUrlAsync("It doesn't matter."));
//        }

//        [Test]
//        public void GoodDomainModelCreator_CreateGoodDomainModelFromUrlAsync_HtmlMarkupGettingErrorArgumentException()
//        {
//            // Arrange
//            Mock<IStoreAdapter> parsingAdapter = new Mock<IStoreAdapter>();
//            parsingAdapter.Setup(pa => pa.GetItemInfoByURLAsync(It.IsAny<URLInfo>()))
//                .Throws<HttpRequestException>();

//            Mock<IAdapterResolver> adapterResolver = new Mock<IAdapterResolver>();
//            adapterResolver.Setup(ar => ar.Resolve(It.IsAny<string>()))
//                .Returns(parsingAdapter.Object);

//            // Assert
//            Assert.ThrowsAsync<ArgumentException>(async () => await
//            new AddUpdateGoodsService(
//                    this._goodService.Object,
//                    this._mapper.Object,
//                    this._urlParser.Object,
//                adapterResolver.Object)
//                .CreateGoodDomainModelFromUrlAsync("It doesn't matter."));
//        }

//        [Test]
//        public void GoodDomainModelCreator_CreateGoodDomainModelFromUrlAsync_InfoAtStoreNotExistArgumentException()
//        {
//            // Arrange
//            Mock<IStoreAdapter> parsingAdapter = new Mock<IStoreAdapter>();
//            parsingAdapter.Setup(pa => pa.GetItemInfoByURLAsync(It.IsAny<URLInfo>()))
//                .ReturnsAsync((ItemInfo)null);

//            Mock<IAdapterResolver> adapterResolver = new Mock<IAdapterResolver>();
//            adapterResolver.Setup(ar => ar.Resolve(It.IsAny<string>()))
//                .Returns(parsingAdapter.Object);

//            // Assert
//            Assert.ThrowsAsync<ArgumentException>(async () => await
//            new AddUpdateGoodsService(
//                    this._goodService.Object,
//                    this._mapper.Object,
//                    this._urlParser.Object,
//                adapterResolver.Object)
//                .CreateGoodDomainModelFromUrlAsync("It doesn't matter."));
//        }
//    }
//}