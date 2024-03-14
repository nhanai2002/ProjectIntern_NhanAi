using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Services.FileUploadService
{
    public interface IFileUploadService
    {
        Task<string> UpLoadFileAsync(IFormFile file);
    }
}
