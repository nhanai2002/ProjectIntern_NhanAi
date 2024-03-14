using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Services.FileUploadService
{
    public class CloudinaryUploadService : IFileUploadService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryUploadService(IOptions<CloudinarySetting> cloud)
        {
            var acc = new Account(cloud.Value.CloudName, cloud.Value.ApiKey, cloud.Value.ApiSecret);
            cloudinary = new Cloudinary(acc);
        }
        public async Task<string> UpLoadFileAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream)
                    };

                    uploadResult = await cloudinary.UploadAsync(uploadParams);
                }
            }
            return uploadResult.Url.ToString();
        }

    }
}
