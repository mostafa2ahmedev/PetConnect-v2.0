using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PetConnect.BLL.Common.AttachmentServices
{

    public class AttachmentService : IAttachmentService
    {
        private readonly List<string> _allowedExtensions = new() { ".png", ".jpg", ".jpeg" , ".pdf" };

        private const int _allowedMaxSize = 2_097_152; 

        public async Task<string?> UploadAsync(IFormFile file, string folderName)
        {
           
            var extension = Path.GetExtension(file.FileName);

            if (!_allowedExtensions.Contains(extension))
                return null;
            if (file.Length > _allowedMaxSize)
                return null;
            
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\assets", folderName);
            
            var fileName = $"{Guid.NewGuid()}{extension}";
            
            var filePath = Path.Combine(folderPath, fileName);
           
            using var fileStream = new FileStream(filePath, FileMode.Create);
           
            await file.CopyToAsync(fileStream);

            return fileName; 
        }
        public async Task<string> UploadAsync(byte[] fileBytes, string fileName, string folderName)
        {
            var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fullPath = Path.Combine(folderPath, newFileName);
            await File.WriteAllBytesAsync(fullPath, fileBytes); 

            return newFileName;
        }
        public bool Delete(string filePath)
        {

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
        public async Task<string?> ReplaceAsync(string? oldRelativePath, IFormFile newFile, string folderName)
        {
             //Delete old file
            if (!string.IsNullOrWhiteSpace(oldRelativePath))
            {
                string oldFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldRelativePath.TrimStart('/'));
                Delete(oldFullPath);
            }
            //Upload new file
            return await UploadAsync(newFile, folderName);
        }
    }
}

