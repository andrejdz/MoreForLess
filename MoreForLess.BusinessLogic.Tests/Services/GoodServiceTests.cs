using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Moq;
using NUnit.Framework;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services;
using MoreForLess.BusinessLogic.Validation;
using MoreForLess.DataAccess.EF;
using MoreForLess.DataAccess.Entities;

namespace MoreForLess.BusinessLogic.Tests.Services
{
    [TestFixture]
    public class GoodServiceTests
    {
        private Mock<DbSet<Good>> _dbSetGoods;
        private Mock<DbSet<Currency>> _dbSetCurrencies;
        private Mock<DbSet<Shop>> _dbSetShops;

        private Mock<ApplicationDbContext> _context;

        private GoodDomainModel _goodForCreate;

        private List<Currency> _currencies;
        private List<Good> _goods;
        private List<Shop> _shops;

        private Mock<IMapper> _mockMapper;

        private readonly IValidator<GoodDomainModel> _validator = new GoodDomainModelValidator();

        [SetUp]
        public void Initialize()
        {
            this._currencies = new List<Currency>
            {
                new Currency {Name = "BYN"},
                new Currency {Name = "USD"},
                new Currency {Name = "EUR"}
            };

            this._shops = new List<Shop>()
            {
                new Shop { Name = "ONLINER" },
                new Shop { Name = "AMAZON" },
                new Shop { Name = "ALIEXPRESS" },
                new Shop { Name = "EBAY" },
            };

            this._goods = new List<Good>
            {
                new Good
                {
                    Id=1,
                    Name="Apple MacBook Air 13",
                    Price = 2050M,
                    IdGoodOnShop="B00IOY8XWQ",
                    LinkOnProduct = "https://catalog.onliner.by/notebook/apple/mqd32",
                    LinkOnPicture = "https://content2.onliner.by/catalog/device/header/69fd1638fafefbe0ecc02ab7fd9391f0.jpeg",
                    CurrencyId=1,
                    ShopId =1,
                    Currency = this._currencies[0],
                    Shop = this._shops[0],
                },
                new Good
                {
                    Id=2,
                    Name="Samsung Galaxy Note8 Dual SIM 64GB",
                    Price = 1799M,
                    IdGoodOnShop="dgte5466",
                    LinkOnProduct = "https://catalog.onliner.by/mobile/samsung/galaxynote8",
                    LinkOnPicture = "https://content2.onliner.by/catalog/device/header/670e197659483f1565345d5f9f5e2461.jpeg",
                    CurrencyId=1,
                    ShopId =1,
                    Currency = this._currencies[0],
                    Shop = this._shops[0],
                }
            };

            this._goodForCreate = new GoodDomainModel
            {
                Name = "Canon EOS 700D Kit 18-55 IS STM",
                Price = 1090M,
                LinkOnProduct = "https://catalog.onliner.by/photo/canon/eos700d1855isstm",
                LinkOnPicture = "https://content2.onliner.by/catalog/device/header/76feafa9888ce98634d48176b55f8eb4.jpg",
                IdGoodOnShop = "76hjh87",
                ShopName = "ONLINER",
                CurrencyName = "BYN"

            };

            this._mockMapper = new Mock<IMapper>();

            this._mockMapper.Setup(x => x.Map<Good>(It.IsAny<GoodDomainModel>()))
                .Returns((GoodDomainModel source) => new Good
                {
                    Name = source.Name,
                    Price = source.Price,
                    LinkOnProduct = source.LinkOnProduct,
                    LinkOnPicture = source.LinkOnPicture,
                    IdGoodOnShop = source.IdGoodOnShop,
                });

            this._mockMapper.Setup(x => x.Map<GoodDomainModel>(It.IsAny<Good>()))
                .Returns(() => new GoodDomainModel
                {
                    Name = this._goods[0].Name,
                    CurrencyName = this._goods[0].Currency.Name,
                    Id = this._goods[0].Id,
                    IdGoodOnShop = this._goods[0].IdGoodOnShop,
                    LinkOnPicture = this._goods[0].LinkOnPicture,
                    LinkOnProduct = this._goods[0].LinkOnProduct,
                    Price = this._goods[0].Price,
                    ShopName = this._goods[0].Shop.Name
                });

            this._context = new Mock<ApplicationDbContext>();
            this._dbSetGoods = new Mock<DbSet<Good>>().SetupData(this._goods);
            this._dbSetCurrencies = new Mock<DbSet<Currency>>().SetupData(this._currencies);
            this._dbSetShops = new Mock<DbSet<Shop>>().SetupData(this._shops);
            this._context.Setup(m => m.Goods).Returns(this._dbSetGoods.Object);
            this._context.Setup(m => m.Currencies).Returns(this._dbSetCurrencies.Object);
            this._context.Setup(m => m.Shops).Returns(this._dbSetShops.Object);
        }

        [Test]
        public async Task GoodService_CreateAsync_AddNewGoodToDataBase()
        {
            var service = new GoodService(this._context.Object, this._mockMapper.Object, this._validator);
            var actual = await service.CreateAsync(this._goodForCreate);
            Assert.AreEqual("Canon EOS 700D Kit 18-55 IS STM", this._dbSetGoods.Object.FirstOrDefaultAsync(u => u.Price == 1090M).Result.Name);
        }

        [Test]
        public async Task GoodService_CreateAsync_EmptyGoodNameThrowArgumentException()
        {
            var service = new GoodService(this._context.Object, this._mockMapper.Object, this._validator);
            var actual = await service.CreateAsync(this._goodForCreate);
            this._goodForCreate.Name = "";
            var ex = Assert.Throws(typeof(AggregateException),
                () => service.CreateAsync(this._goodForCreate).Wait());
            Assert.That(ex.InnerException, Is.TypeOf<ArgumentException>());
        }

        [Test]
        public async Task GoodService_CreateAsync_InvalidCurrencyNameThrowArgumentException()
        {
            var service = new GoodService(this._context.Object, this._mockMapper.Object, this._validator);
            var actual = await service.CreateAsync(this._goodForCreate);
            this._goodForCreate.CurrencyName = "Currency";
            var ex = Assert.Throws(typeof(AggregateException),
                () => service.CreateAsync(this._goodForCreate).Wait());
            Assert.That(ex.InnerException, Is.TypeOf<ArgumentException>());
        }

        [Test]
        public async Task GoodService_CreateAsync_InvalidShopNameThrowArgumentException()
        {
            var service = new GoodService(this._context.Object, this._mockMapper.Object, this._validator);
            var actual = await service.CreateAsync(this._goodForCreate);
            this._goodForCreate.ShopName = "Name";
            var ex = Assert.Throws(typeof(AggregateException),
                () => service.CreateAsync(this._goodForCreate).Wait());
            Assert.That(ex.InnerException, Is.TypeOf<ArgumentException>());
        }

        [Test]
        public async Task GoodService_DeleteAsync_DeleteGoodWithId2FromDataBase()
        {
            int id = 2;
            var service = new GoodService(this._context.Object, this._mockMapper.Object, this._validator);
            await service.DeleteAsync(id);
            Assert.AreEqual(1, this._dbSetGoods.Object.Count());
        }

        [Test]
        public void GoodService_DeleteAsync_IncorrectIdReturnAgrumentNullExceprion()
        {
            int id = 4;
            var service = new GoodService(this._context.Object, this._mockMapper.Object, this._validator);
            var ex = Assert.Throws(typeof(AggregateException),
                () => service.DeleteAsync(id).Wait());
            Assert.That(ex.InnerException, Is.TypeOf<ArgumentException>());
        }

        [Test]
        public async Task GoodService_IsItemExist_ExistGoodReturnTrue()
        {
            string url = "https://catalog.onliner.by/notebook/apple/mqd32";
            var service = new GoodService(this._context.Object, this._mockMapper.Object, this._validator);
            bool isExist = await service.IsItemExistAsync(url);
            Assert.AreEqual(true, isExist);
        }

        [Test]
        public async Task GoodService_IsItemExist_InvalidGoodReturnFalse()
        {
            string url = "https://catalog.onliner.by/notebook/apple/mqd3211";
            var service = new GoodService(this._context.Object, this._mockMapper.Object, this._validator);
            bool isExist = await service.IsItemExistAsync(url);
            Assert.AreEqual(false, isExist);
        }

        [Test]
        public async Task GoodService_GetGoodByIdAsync_ReturnFoodWithId1()
        {
            int id = 1;
            var service = new GoodService(this._context.Object, this._mockMapper.Object, this._validator);
            var expectedGood = await service.GetGoodByIdAsync(id);
            Assert.AreEqual("Apple MacBook Air 13", expectedGood.Name);
        }

        [Test]
        public async Task GoodService_GetAllGoodsAsync_ReturnedAllGoods()
        {
            var service = new GoodService(this._context.Object, this._mockMapper.Object, this._validator);
            var expectedGood = await service.GetAllGoodsAsync();
            Assert.AreEqual("Apple MacBook Air 13", expectedGood.FirstOrDefault(u => u.Id == 1).Name);
        }
    }
}