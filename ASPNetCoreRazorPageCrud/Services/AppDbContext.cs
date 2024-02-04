using ASPNetCoreRazorPageCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreRazorPageCrud.Services;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }

}