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
                    Name = "Accessories and Supplies",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Camera and Photo",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Car and Vehicle Electronics",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Cell Phones and Accessories",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Computers and Accessories",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Electronics Warranties",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "GPS and Navigation",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Headphones",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Home Audio",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Office Electronics",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Portable Audio and Video",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Security and Surveillance",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Service Plans",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Television and Video",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Video Game Consoles and Accessories",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Video Projectors",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "Wearable Technology",
                    ParentId = "1"
                },
                new Category()
                {
                    Name = "eBook Readers and Accessories",
                    ParentId = "1"
                },
            };

            foreach (var item in categories)
            {
                context.Categories
                    .AddOrUpdate(c => new { c.Name, c.ParentId }, item);
            }

            context.SaveChanges();

            var rootCategory = new StoreCategory()
            {
                IdAtStore = "172282",
                Name = "Electronics",
                ParentIdAtStore = null,
                ShopId = 1,
            };

            context.StoreCategories
                .AddOrUpdate(s => new { s.IdAtStore, s.Name, s.ParentIdAtStore, s.ShopId }, rootCategory);

            context.SaveChanges();

            var rootCategory2 = new StoreCategory()
            {
                IdAtStore = "493964",
                Name = "Categories",
                ParentIdAtStore = "172282",
                ShopId = 1,
            };

            context.StoreCategories
                .AddOrUpdate(s => new { s.IdAtStore, s.Name, s.ParentIdAtStore, s.ShopId }, rootCategory2);

            context.SaveChanges();

            List<StoreCategory> storeCategories = new List<StoreCategory>()
            {
                new StoreCategory()
                {
                    IdAtStore = "281407",
                    Name = "Accessories & Supplies",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "502394",
                    Name = "Camera & Photo",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "3248684011",
                    Name = "Car & Vehicle Electronics",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "2811119011",
                    Name = "Cell Phones & Accessories",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "541966",
                    Name = "Computers & Accessories",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "2242348011",
                    Name = "Electronics Warranties",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "172526",
                    Name = "GPS & Navigation",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "172541",
                    Name = "Headphones",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "667846011",
                    Name = "Home Audio",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "172574",
                    Name = "Office Electronics",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "172623",
                    Name = "Portable Audio & Video",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "524136",
                    Name = "Security & Surveillance",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "16285901",
                    Name = "Service Plans",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "1266092011",
                    Name = "Television & Video",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "7926841011",
                    Name = "Video Game Consoles & Accessories",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "300334",
                    Name = "Video Projectors",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "10048700011",
                    Name = "Wearable Technology",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
                new StoreCategory()
                {
                    IdAtStore = "2642125011",
                    Name = "eBook Readers & Accessories",
                    ShopId = 1,
                    ParentIdAtStore = "493964",
                },
            };

            foreach (var item in storeCategories)
            {
                context.StoreCategories
                    .AddOrUpdate(s => new { s.IdAtStore, s.Name, s.ParentIdAtStore, s.ShopId }, item);
            }

            context.SaveChanges();
        }
    }
}
