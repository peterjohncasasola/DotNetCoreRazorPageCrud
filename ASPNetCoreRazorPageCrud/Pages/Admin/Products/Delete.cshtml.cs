using ASPNetCoreRazorPageCrud.Helpers;
using ASPNetCoreRazorPageCrud.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPNetCoreRazorPageCrud.Pages.Admin.Products
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DeleteModel(IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public void OnGet(int? id)
        {
            var product = _context.Products.Find(id);

            if (product is null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            if (product is not null)
            {
                
                if (product.ImageFileName is not null)
                {
                    _webHostEnvironment.RemoveFile("/images/products/" + product.ImageFileName);
                }

                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            Response.Redirect("/Admin/Products/Index");
        }
    }
}
