using Core.Extensions;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Application.Services
{
    public class ExportService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadDirectory;
        public const string SaveFolderName = "files";
        public ExportService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadDirectory = Path.Combine(environment.WebRootPath, SaveFolderName);
        }

        /// <summary>
        /// Exporting file as excel and returning path 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public string GenerateExcel<T>(List<T> list, string path, string fileName = "report")
        {
            if (path[^1] != '\\')
            {
                path += "\\";
            }
            path = Path.Combine(_uploadDirectory, path);

            fileName = Path.GetFileNameWithoutExtension(fileName) + "_" + DateTime.Now.ToTimeStamp() +
                       GetUniqueString() + ".xlsx";

            string fileDirectory = Path.Combine(path, fileName);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(list, true);

                CreateIfMissing(path);
                // Create excel file on physical disk  
                FileStream objFileStrm = File.Create(fileDirectory);
                objFileStrm.Close();

                byte[] data = package.GetAsByteArray();
                File.WriteAllBytes(fileDirectory, data);
            }
            return fileDirectory;

        }


        private void CreateIfMissing(string path)
        {
            var folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        private void Delete(params string[] paths)
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
        private string GetUniqueString()
        {
            return "_" + Guid.NewGuid().ToString().Substring(0, 4);
        }
    }
}
