using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PetConnect.BLL.Common.AttachmentServices
{
    public interface IAttachmentService
    {
        Task<string?> UploadAsync(IFormFile file, string folderName);
        Task<string?> ReplaceAsync(string? oldRelativePath, IFormFile newFile, string folderName);
        Task<string> UploadAsync(byte[] fileBytes, string fileName, string folderName);
        bool Delete(string filePath);
    }
}
