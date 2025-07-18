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

        private const int _allowedMaxSize = 2_097_152; //Can only take bytes

        public async Task<string?> UploadAsync(IFormFile file, string folderName)
        {
            //Get the extension name from the filename (with dot)
            var extension = Path.GetExtension(file.FileName);

            if (!_allowedExtensions.Contains(extension))
                return null;
            if (file.Length > _allowedMaxSize)
                return null;
            //Get Folder Path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\assets", folderName);
            //The Unique Name for each image
            var fileName = $"{Guid.NewGuid()}{extension}";
            // file location
            var filePath = Path.Combine(folderPath, fileName);
            //file steram is an un managed resource so i must use using statment
            using var fileStream = new FileStream(filePath, FileMode.Create);
            //copy the file that is passed to the function which is the image to the file stream to save it.
            await file.CopyToAsync(fileStream);

            return fileName; // To be saved in database
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

