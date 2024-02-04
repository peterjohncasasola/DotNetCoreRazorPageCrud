

#nullable disable

using ASPNetCoreRazorPageCrud.Helpers;
using ASPNetCoreRazorPageCrud.Models;
using ASPNetCoreRazorPageCrud.Services;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
namespace ASPNetCoreRazorPageCrud.Migrations
{
    /// <inheritdoc />
    public partial class SeedDummyProductData : Migration
    {
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var appHelper = new AppContextHelper();

            var dbContext = appHelper.CreateAppContext();
            var faker = new Faker<Product>()
                .RuleFor(p => p.Brand, f => f.Commerce.ProductMaterial())
                .RuleFor(p => p.Category, f => f.Commerce.Categories(1).FirstOrDefault())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => f.Finance.Amount(0, 16, 2))
                .RuleFor(p => p.CreatedAt, f => DateTime.Now)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName());

            var dummyData = faker.Generate(100);
            dbContext.Products.AddRange(dummyData);
            
            dbContext.SaveChanges();
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE [dbo].[Products]", false);
        }
    }
}
