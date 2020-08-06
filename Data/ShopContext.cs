using Microsoft.EntityFrameworkCore;
using DotShopApi.Models;

/*
    Normally I would use context with models that are mostly related with eachother in a certain way.
    
    For example:
    If I have the Products, Manufacturer and Brand models. Then I would put them under the ProductContext.
    If I have the User, Profile and Address models, Then I would put them under UserContext.

    For this scope, I just put the current two models under ShopContext for simplicity
*/

namespace DotShopApi.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}