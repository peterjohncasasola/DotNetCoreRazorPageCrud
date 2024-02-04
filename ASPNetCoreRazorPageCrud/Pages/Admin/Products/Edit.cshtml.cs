using ASPNetCoreRazorPageCrud.Helpers;
using ASPNetCoreRazorPageCrud.Models;
using ASPNetCoreRazorPageCrud.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPNetCoreRazorPageCrud.Pages.Admin.Products
{
    public class EditModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _context;

        [BindProperty]
        public ProductDto ProductDto { get; set; } = new ProductDto();

        public Product Product { get; set; } = new Product();

        public EditModel(IWebHostEnvironment environment, AppDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        public void OnGet(int? id)
        {
            if (id == null) Response.Redirect("/Admin/Products/Index");

            var product = _context.Products.Find(id);

            if (product is null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            ProductDto.Price = product.Price;
            ProductDto.Brand = product.Brand;
            ProductDto.Name = product.Name;
            ProductDto.Description = product.Description;
            ProductDto.Category = product.Category;

            Product = product;
        }

        public string Message = "";
        public bool IsValid = false;
        public string Alert = "";

        public void OnPost(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            var product = _context.Products.Find(id);

            if (product is null) {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            if (!ModelState.IsValid)
            {
                Message = "Please provide all the required fields";
                IsValid = false;
                Alert = "alert-warning";
            }
            else
            {
                var newFileName = $"{ProductDto.Name}_{ProductDto.Brand}";
                newFileName += Path.GetExtension(ProductDto.ImageFile?.FileName);

                if (product.ImageFileName is not null)
                {
                    _environment.RemoveFile("/images/products/" + product.ImageFileName);
                }

                if (ProductDto.ImageFile is not null)
                {
                    var imagePath = _environment.WebRootPath + "/images/products/" + newFileName;
                    using var stream = System.IO.File.Create(imagePath);
                    ProductDto.ImageFile.CopyTo(stream);
                }

                product.Price = ProductDto.Price;
                product.ImageFileName = newFileName;
                product.Name = ProductDto.Name;
                product.Category = ProductDto.Category;
                product.Description = ProductDto.Description;
                product.Brand = ProductDto.Brand;

                _context.SaveChanges();

                Alert = "alert-success";
                IsValid = true;

                Product = product;
                Message = "Product updated successfully";

                Response.Redirect("/Admin/Products/Index");

            }
        }
    }
}
