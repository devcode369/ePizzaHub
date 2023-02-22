using ePizzaHub.WebUI.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace ePizzaHub.WebUI.Helpers
{
    public class FileHelper : IFileHelper
    {
        IWebHostEnvironment _webHostEnvironment;

        public FileHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment= webHostEnvironment;
        }
        public void DelteFile(string imageUrl)
        {
            if(File.Exists(_webHostEnvironment.WebRootPath+imageUrl))
            {
                File.Delete(_webHostEnvironment.WebRootPath+imageUrl);
            }
        }

        public string UploadFile(IFormFile file)
        {
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            bool exists=Directory.Exists(uploads);
            if(!exists)
                Directory.CreateDirectory(uploads);
            //saving file
            var fileName = GenerateFileName(file.FileName);
            var fileStream=new FileStream(Path.Combine(uploads,fileName), FileMode.Create);
            file.CopyToAsync(fileStream);

            return "/Images/" + fileName;
        }

        private string GenerateFileName(string fileName)
        {
            string[] strName = fileName.Split('.');
            string strFileName = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }

    }
}
