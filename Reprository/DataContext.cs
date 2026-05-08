using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockManagement.Models;
namespace StockManagement.Reprository
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<ProductModel> Products { set; get; }
        public DbSet<WarehouseTransactionModel> WarehouseTransactions { set; get; }
    }
}
