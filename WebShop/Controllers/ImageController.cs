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
        public ActionResult ProductImages(int productId)
        {
            var imgProduct = _uow.ProductImageRepository
                .BuildQuery(x => x.ProductId == productId)
                .Select(x => x.ImageId).ToList();
            var rs = _uow.ImageRepository
                .BuildQuery(x => imgProduct.Contains(x.ImageId))
                .Select(x => _mapper.Map<ImageViewModel>(x))
                .ToList();
            ViewData["ProductId"] = productId;
            return View(rs);
        }

        [Display(Name = "Thêm ảnh cho sản phẩm")]
        public ActionResult CreateProductImages(int productId)
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
        public async Task<ActionResult> CreateProductImages(ProductImageCrudModel imgCrud)
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
                catch
                {
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
        public async Task<ActionResult> DeleteProductImages(int id, int productId)
        {
            var imgProduct = _uow.ProductImageRepository.BuildQuery(x => x.ImageId == id).AsNoTracking().FirstOrDefault();
            _uow.ProductImageRepository.Delete(imgProduct);
            await _uow.CommitAsync();
            return RedirectToAction("ProductImage", productId);
        }

        [Display(Name = "Chỉnh sửa ảnh cho sản phẩm")]
        public ActionResult EditProductImages(int id, int productId) 
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
        public async Task<ActionResult> EditProductImages(ImageCrudModel imgCrud)
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
