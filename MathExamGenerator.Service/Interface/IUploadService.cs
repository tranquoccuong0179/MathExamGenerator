using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MathExamGenerator.Service.Interface
{
    public interface IUploadService
    {
        Task<string> UploadImage(IFormFile file);

        Task<string> UploadToGoogleDriveAsync(IFormFile fileToUpload);
    }
}
