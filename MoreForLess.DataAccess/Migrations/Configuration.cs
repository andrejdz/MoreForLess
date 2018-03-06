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
                new Currency() { Name = "USD" },
                new Currency() { Name = "EUR" }
            };

            foreach (var item in currencyList)
            {
                context.Currencies
                    .AddOrUpdate(c => new { c.Name }, item);
            }

            List<Shop> shopList = new List<Shop>()
            {
                new Shop() { Name = "AMAZON" },
                new Shop() { Name = "EBAY" }
            };

            foreach (var item in shopList)
            {
                context.Shops
                    .AddOrUpdate(s => new { s.Name }, item);
            }

            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Electronics",
                    ParentId = "0"
                },
                new Category()
                {
                    Name = "Computers and Accessories",
                    ParentId = "1"
                },
            };

            foreach (var item in categories)
            {
                context.Categories
                    .AddOrUpdate(c => new { c.Name, c.ParentId }, item);
            }

            context.SaveChanges();

            List<StoreCategory> storeCategories = new List<StoreCategory>()
            {
                new StoreCategory()
                {
                    IdAtStore = "172282",
                    Name = "Electronics",
                    ParentIdAtStore = "0",
                    ShopId = 1,
                    CategoryId = 1
                },
                new StoreCategory()
                {
                    IdAtStore = "493964",
                    Name = "Categories",
                    ParentIdAtStore = "172282",
                    ShopId = 1,
                    CategoryId = 1
                },
            };

            foreach (var item in storeCategories)
            {
                context.StoreCategories
                    .AddOrUpdate(s => new { s.IdAtStore, s.Name, s.ParentIdAtStore, s.ShopId }, item);
            }

            var good = new Good()
            {
                Name = "Apple MacBook Pro 15'' Retina",
                Price = 420.25M,
                IdGoodOnShop = "BBBFFFGGGHHH",
                LinkOnProduct = "LinkOnProduct",
                LinkOnPicture = "LinkOnPicture",
                CategoryIdOnShop = "493964",
                CurrencyId = 1,
                ShopId = 1,
                CategoryId = 1
            };

            context.Goods
                .AddOrUpdate(g => new { g.Name, g.Price, g.IdGoodOnShop, g.LinkOnProduct, g.CategoryIdOnShop, g.ShopId }, good);

            context.SaveChanges();

            var comment = new Comment
            {
                Text = "Very useful product. I'm satisfied",
                GoodId = 1
            };

            context.Comments
                .AddOrUpdate(c => new { c.Text, c.GoodId }, comment);

            //var score = new Score
            //{
            //    Value = 4,
            //    GoodId = 1
            //};

            //context.Scores
            //    .AddOrUpdate(score);

            context.SaveChanges();
        }
    }
}
