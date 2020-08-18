namespace BookStore.UI.Services
{
    using BlazorInputFile;
    using BookStore.UI.Contracts;
    using Microsoft.AspNetCore.Hosting;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment env;
        public FileUpload(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public void RemoveFile(string picName)
        {
            var path = $"{env.WebRootPath}\\uploads\\{picName}";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task UploadFile(IFileListEntry file, string picName)
        {
            try
            {
                var ms = new MemoryStream();
                await file.Data.CopyToAsync(ms);

                var path = $"{env.WebRootPath}\\uploads\\{picName}";

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    ms.WriteTo(fs);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UploadFile(IFileListEntry file, MemoryStream msFile, string picName)
        {
            try
            {
                var path = $"{env.WebRootPath}\\uploads\\{picName}";

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    msFile.WriteTo(fs);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
