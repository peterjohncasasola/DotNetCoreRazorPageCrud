using ASPNetCoreRazorPageCrud.Models;

namespace ASPNetCoreRazorPageCrud.Helpers
{
    public static class FilePathHelper
    {
        
        public static void RemoveFile(this IWebHostEnvironment environment, string path)
        {
            var filePath = environment.WebRootPath + path;
            System.IO.File.Delete(filePath);
        }
    }
}
