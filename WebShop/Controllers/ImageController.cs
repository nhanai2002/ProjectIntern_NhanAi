using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebShop.Models;
using WebShop.Permission;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.Repositories;
using WebShopCore.Services.FileUploadService;
using WebShopCore.ViewModel.Image;

namespace WebShop.Controllers
{
    [LoginRequired]
    [Display(Name = "Hình ảnh")]
    public class ImageController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;

        public ImageController(IUnitOfWork uow, IFileUploadService fileUploadService, IMapper mapper)
        {
            _uow = uow;
            _fileUploadService = fileUploadService;
            _mapper = mapper;
        }


        [Display(Name = "Xem hình ảnh của sản phẩm")]
        public IActionResult ProductImages(int productId)
        {
            try
            {
                var product = _uow.ProductRepository.FirstOrDefault(x => x.ProductId == productId);
                if (product != null)
                {
                    var imgProduct = _uow.ProductImageRepository
                                        .BuildQuery(x => x.ProductId == product.ProductId)
                                        .Select(x => x.ImageId)
                                        .ToList();
                    var rs = _uow.ImageRepository
                                        .BuildQuery(x => imgProduct.Contains(x.ImageId))
                                        .Select(x => _mapper.Map<ImageViewModel>(x))
                                        .ToList();
                    ViewData["ProductId"] = product.ProductId;
                    ViewData["ProductCode"] = product.Code;
                    return View(rs);
                }
                else
                {
                    return View("Error", "Sản phẩm không tồn tại!");
                }
            }
            catch
            {
                return View("Error", "Đã xảy ra lỗi!");
            }
        }



        [Display(Name = "Thêm ảnh cho sản phẩm")]
        public IActionResult CreateProductImages(int productId)
        {
            var modelError = new ErrorViewModel();
            var product = _uow.ProductRepository.FirstOrDefault(x => x.ProductId == productId);
            if(product != null)
            {
                var rs = new ProductImageCrudModel();
                rs.ProductId = productId;
                return View(rs);
            }
            else
            {
                modelError.ErrorMessage = "Lỗi không tìm thấy sản phẩm";
                return View("Error", modelError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductImages(ProductImageCrudModel imgCrud)
        {
            if (ModelState.IsValid)
            {
                var error = new ErrorViewModel();
                try
                {
                    var urls = new List<string>();

                    foreach (var image in imgCrud.FileImages)
                    {
                        var url = await _fileUploadService.UpLoadFileAsync(image);
                        urls.Add(url);
                    }
                    foreach (var url in urls)
                    {
                        var model = new Image
                        {
                            Url = url,
                            IsActive = true,
                        };
                        await _uow.ImageRepository.AddAsync(model);
                        await _uow.CommitAsync();

                        var productImg = new ProductImage
                        {
                            ProductId = imgCrud.ProductId,
                            ImageId = model.ImageId
                        };
                        await _uow.ProductImageRepository.AddAsync(productImg);

                    }
                    await _uow.CommitAsync();
                    return RedirectToAction("ProductImages", new { productId = imgCrud.ProductId });
                }
                catch (Exception ex)
                {
                    var a = ex.InnerException;
                    var b = ex.Message;
                    error.ErrorMessage = "Lỗi khi tạo hình ảnh sản phẩm";
                    return View("Error", error);
                }

            }
            else
            {
                return View(imgCrud);
            }
        }



        [Display(Name = "Xóa ảnh của sản phẩm")]
        [HttpPost]
        public async Task<IActionResult> DeleteProductImages(int id, int productId)
        {
            try
            {
                var imgProduct = _uow.ProductImageRepository.BuildQuery(x => x.ImageId == id && x.ProductId == productId).AsNoTracking().FirstOrDefault();
                _uow.ProductImageRepository.Delete(imgProduct);
                await _uow.CommitAsync();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }



        [Display(Name = "Chỉnh sửa ảnh cho sản phẩm")]
        public IActionResult EditProductImages(int id, int productId) 
        {
            var error = new ErrorViewModel();
            var img = _uow.ImageRepository.FirstOrDefault(x => x.ImageId == id);
            var product = _uow.ProductRepository.FirstOrDefault(x => x.ProductId == productId);
            var rs = _mapper.Map<ImageCrudModel>(img);
            rs.ProductId = product.ProductId;
            if (img == null)
            {
                error.ErrorMessage = "Lỗi không tìm thấy hình ảnh";
                return View("Error", error);
            }
            else
            {
                return View(rs);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProductImages(ImageCrudModel imgCrud)
        {
            if (ModelState.IsValid)
            {
                var error = new ErrorViewModel();
                try
                {
                    if(imgCrud.FileImage != null)
                    {
                        var img = _uow.ImageRepository.FirstOrDefault(x => x.ImageId == imgCrud.ImageId);
                        var uploadResult = await _fileUploadService.UpLoadFileAsync(imgCrud.FileImage);
                        img.Url = uploadResult;
                        img.UpdatedAt = DateTime.Now;
                        await _uow.CommitAsync();
                        return RedirectToAction("ProductImages", new { productId = imgCrud.ProductId });
                    }
                }
                catch (Exception ex)
                {
                    error.ErrorMessage = "Lỗi khi sửa hình ảnh";
                    return View("Error", error);
                }
            }
            return View(imgCrud);
        }
    }
}
