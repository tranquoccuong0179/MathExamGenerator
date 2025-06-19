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

namespace MathExamGenerator.Service.Implement
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;

        public UploadService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
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
    }
}
