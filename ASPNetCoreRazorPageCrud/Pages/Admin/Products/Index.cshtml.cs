using ASPNetCoreRazorPageCrud.Models;
using ASPNetCoreRazorPageCrud.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPNetCoreRazorPageCrud.Pages.Admin.Products
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        
        public List<Product> Products { get; set; } = new();
        public int PageIndex = 1;
        public int TotalPages = 1;
        public int pageSize = 5;
        private int perPage = 10;

        
        public string Search = string.Empty;
        public string OrderBy = string.Empty;
        public string SortBy = "Id";

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }
        public void OnGet(int? pageIndex = 1, string search = "", string sortBy = "", string orderBy="", int? pageSize = 10)
        {

            string[] validColumns = { "Id", "Name", "Brand", "Category", "Price", "CreatedAt" };
            string[] validOrderBy = { "desc", "asc" };

            var query = _context.Products.OrderByDescending(p => sortBy).AsQueryable();

            if (!validColumns.Contains(sortBy)) SortBy = "Id";

            if (!validOrderBy.Contains(orderBy)) OrderBy = "desc";

            if (search is not null)
            {
                Search = search;
                query = query.Where(q => q.Name!.Contains(search) || q.Brand!.Contains(search) || q.Description!.Contains(search));
            }

            OrderBy = orderBy;
            SortBy = sortBy;

            var count = query.Count();

            PageIndex = pageIndex ?? 1;
            perPage = pageSize ?? 10;

            TotalPages = (int) Math.Ceiling((double)count / perPage);

            query = query.Skip((PageIndex - 1) * perPage).Take(perPage);

            Products = query.ToList();
        }
    }
}
