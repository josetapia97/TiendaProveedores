using API.FurnitoreStore.Shared;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace API.FurnitoreStore.Data
{
    public class APIFurnitureStoreContext : IdentityDbContext
    {
        //constructor para que toda la config de EF se realice
        public APIFurnitureStoreContext(DbContextOptions options) : base(options) { }


        //DB set: representaciones de la tabla en codigo || utiliza shared
        public DbSet<Client> Clients {  get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        //al utsar SQLite se usa este override
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }

        //para que la tabla OrderDetail tenga de PK (idProduct+idOrder)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderDetail>()
                .HasKey(pk => new { pk.OrderId,pk.ProductId});
        }
    }
}
