using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Google.Apis.Drive.v3;
using MathExamGenerator.Model.Payload.Settings;
using Microsoft.Extensions.Options;

namespace MathExamGenerator.Service.Implement
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;
        private readonly DriveService _driveService;
        private readonly string _googleDriveFolderId;

        public UploadService(Cloudinary cloudinary,
                             DriveService driveService,
                             IOptions<GoogleDriveSettings> driveSettings)
        {
            _cloudinary = cloudinary;
            _driveService = driveService;
            _googleDriveFolderId = driveSettings.Value.FolderId;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            if (file == null)
            {
                throw new NotFoundException();
            }

            using (var stream = file.OpenReadStream())
            {
                var uploadParam = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, stream),
                    Folder = "image_avt",
                    PublicId = Guid.NewGuid().ToString(),
                    Transformation = new Transformation().Quality("auto:low")
                                                         .FetchFormat("webp")
                                                         .Width(1024)
                                                         .Crop("limit")
                };


                var uploadResult = await _cloudinary.UploadAsync(uploadParam);
                if (uploadResult.StatusCode == HttpStatusCode.OK)
                {
                    return uploadResult.SecureUrl.ToString();
                }
                else
                {
                    throw new Exception("Failed to upload image to Cloudinary.");
                }
            }
        }

        public async Task<string> UploadToGoogleDriveAsync(IFormFile fileToUpload)
        {
            if (fileToUpload == null) throw new NotFoundException();

            var allowedExtensions = new[] { ".docx", ".pdf", ".mov", ".xlsx", ".mp4", ".jpg", ".txt" };
            var ext = Path.GetExtension(fileToUpload.FileName).ToLower();
            if (!allowedExtensions.Contains(ext))
                throw new InvalidOperationException("Định dạng file không được hỗ trợ.");

            var meta = new Google.Apis.Drive.v3.Data.File
            {
                Name = fileToUpload.FileName,
                Parents = new List<string> { _googleDriveFolderId }
            };

            using var stream = fileToUpload.OpenReadStream();
            var request = _driveService.Files.Create(meta, stream, fileToUpload.ContentType);
            request.Fields = "id";
            await request.UploadAsync();

            var file = request.ResponseBody;
            string fileUrl = $"https://drive.google.com/file/d/{file.Id}/view?usp=sharing";
            return fileUrl;
        }
    }
}
