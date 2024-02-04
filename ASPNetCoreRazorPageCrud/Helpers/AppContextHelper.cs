using ASPNetCoreRazorPageCrud.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ASPNetCoreRazorPageCrud.Helpers
{
    public class AppContextHelper
    {
        public AppDbContext CreateAppContext()
        {
            var builder = WebApplication.CreateBuilder();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            var context = new AppDbContext(optionsBuilder.Options);
            
            return context;
        }
    }
}
