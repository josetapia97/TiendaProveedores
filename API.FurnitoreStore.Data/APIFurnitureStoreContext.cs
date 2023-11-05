using API.FurnitoreStore.Shared;
using Microsoft.EntityFrameworkCore;


namespace API.FurnitoreStore.Data
{
    public class APIFurnitureStoreContext : DbContext
    {
        //constructor para que toda la config de EF se realice
        public APIFurnitureStoreContext(DbContextOptions options) : base(options) { }


        //DB set: representaciones de la tabla en codigo || utiliza shared
        public DbSet<Client> Clients {  get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        //al utsar SQLite se usa este override
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }

    }
}
