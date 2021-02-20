using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class UploadService
    {
        private readonly string _uploadDirectory;

        public UploadService(IWebHostEnvironment environment)
        {
            _uploadDirectory = Path.Combine(environment.ContentRootPath, "Uploads");
        }

        public async Task<Dictionary<string, string>> SaveAsync(params IFormFile[] files)
        {
            var pathDic = new Dictionary<string, string>();
            var uniqueString = GetUniqueString();
            CreateIfMissing(_uploadDirectory);
            foreach (var file in files)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var uniquerFileName = fileNameWithoutExtension + uniqueString + fileExtension;

                var path = Path.Combine(_uploadDirectory, uniquerFileName);
                await using (var stream = File.Create(path))
                {
                    await file.CopyToAsync(stream).ConfigureAwait(false);
                }
                pathDic.Add(file.FileName, path);
            }
            return pathDic;
        }

        public void CreateIfMissing(string path)
        {
            var folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        public void Delete(params string[] paths)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
        public string GetUniqueString()
        {
            return "_" + Guid.NewGuid().ToString().Substring(0, 4);
        }
    }
}
