using ASPNetCoreRazorPageCrud.Models;
using ASPNetCoreRazorPageCrud.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPNetCoreRazorPageCrud.Pages.Admin.Products
{
    public class CreateModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _context;

        [BindProperty]
        public ProductDto ProductDto { get; set; } = new ProductDto();

        public CreateModel(IWebHostEnvironment environment, AppDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        public void OnGet()
        {
        }

        public string Message = "";
        public bool IsValid = false;
        public string Alert = "";

        public void OnPost()
        {
            if (ProductDto.ImageFile is null)
                ModelState.AddModelError("ProductDto.ImageFile", "The image file is required");

            if (!ModelState.IsValid)
            {
                Message = "Please provide all the required fields";
                IsValid = false;
                Alert = "alert-warning";
            }
            else
            {
                var newFileName = $"{ProductDto.Name}_{ProductDto.Brand}";
                newFileName = newFileName.Trim();
                newFileName += Path.GetExtension(ProductDto.ImageFile?.FileName);


                var imagePath = _environment.WebRootPath + "/images/products/" + newFileName;
                using (var stream = System.IO.File.Create(imagePath))
                {
                    ProductDto.ImageFile!.CopyTo(stream);
                }

                var product = new Product()
                {
                    Price = ProductDto.Price,
                    Name = ProductDto.Name,
                    Brand = ProductDto.Brand,
                    Category = ProductDto.Category,
                    CreatedAt = DateTime.Now,
                    Description = ProductDto.Description,
                    ImageFileName = newFileName
                };

                _context.Products.Add(product);
                _context.SaveChanges();

                Alert = "alert-success";
                IsValid = true;
                ProductDto.ImageFile = null;
                ProductDto.Name = null;
                ProductDto.Price = 0;
                ProductDto.Description = null;
                ProductDto.Category = null;
                ProductDto.ImageFile = null;
                ModelState.Clear();
                Message = "Product created successfully";

                
                Response.Redirect("/Admin/Products/Index");

            }
        }
    }
}
