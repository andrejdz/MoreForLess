using System.Collections.Generic;
using System.Data.Entity.Migrations;
using MoreForLess.DataAccess.EF;
using MoreForLess.DataAccess.Entities;

namespace MoreForLess.DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
            this.ContextKey = "MoreForLess.DataAccess.EF.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            List<Currency> currencyList = new List<Currency>()
            {
                new Currency() { Name = "BYN" },
                new Currency() { Name = "USD" },
                new Currency() { Name = "EUR" },
                new Currency() { Name = "RUR" },
            };

            foreach (var item in currencyList)
            {
                context.Currencies
                    .AddOrUpdate(c => new { c.Name }, item);
            }

            List<Shop> shopList = new List<Shop>()
            {
                new Shop() { Name = "ONLINER" },
                new Shop() { Name = "AMAZON" },
                new Shop() { Name = "ALIEXPRESS" },
                new Shop() { Name = "EBAY" },
            };

            foreach (var item in shopList)
            {
                context.Shops
                    .AddOrUpdate(s => new { s.Name }, item);
            }

            context.SaveChanges();

            List<Good> goodList = new List<Good>()
            {
                new Good()
                {
                    Name = "Apple MacBook Pro 15'' Retina",
                    Price = 4200,
                    IdGoodOnShop = "notebook/apple/mjlq2",
                    CurrencyId = 1,
                    ShopId = 1,
                    LinkOnProduct = "https://catalog.onliner.by/notebook/apple/mjlq2",
                    LinkOnPicture = "https://content2.onliner.by/catalog/device/header/721ad26018661faa45979b1c9b048fb1.jpg"
                },
                new Good()
                {
                    Name = "Apple iPhone X 64GB",
                    Price = 2600,
                    IdGoodOnShop = "mobile/apple/iphonex",
                    CurrencyId = 1,
                    ShopId = 1,
                    LinkOnProduct = "https://catalog.onliner.by/mobile/apple/iphonex",
                    LinkOnPicture = "https://content2.onliner.by/catalog/device/header/10f55bb2587a0c163150f8efbf57022f.jpeg"
                },
                new Good()
                {
                    Name = "Pay Card Apple iTunes",
                    Price = 4200,
                    IdGoodOnShop = "paymentcard/apple/itunes1000",
                    CurrencyId = 1,
                    ShopId = 1,
                    LinkOnProduct = "https://catalog.onliner.by/paymentcard/apple/itunes1000",
                    LinkOnPicture = "https://content2.onliner.by/catalog/device/header/a191a76e1779e2f0d66793116e349adb.jpeg"
                }
            };

            foreach (var item in goodList)
            {
                context.Goods
                    .AddOrUpdate(g => new { g.Name, g.Price, g.IdGoodOnShop, g.LinkOnProduct }, item);
            }

            context.SaveChanges();
        }
    }
}
