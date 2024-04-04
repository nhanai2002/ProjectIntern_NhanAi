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
using WebShopCore.ViewModel.Category;
using WebShopCore.ViewModel.Product;

namespace WebShop.Controllers
{
    [Display(Name = "Quản lý sản phẩm")]
    [LoginRequired]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProductController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [Display(Name = "Xem sản phẩm")]
        public IActionResult Index()
        {
            var data = _uow.ProductRepository.BuildQuery(x => !x.IsDeleted);
            data = data.Include(x => x.ProductCategories);
           
            var result = _mapper.Map<List<ProductViewModel>>(data.ToList());

            return View(result);
        }

        [Display(Name = "Thêm sản phẩm")]
        public ActionResult Create()
        {
            var model = new ProductCrudModel();
            var error = new ErrorViewModel();
            model.ListCategoryViewModel = 
                _uow.CategoryRepository
                .BuildQuery(x=> !x.IsDeleted && x.IsActive)
                .Select(x=> _mapper.Map<CategoryViewModel>(x)).ToList();
            if(model.ListCategoryViewModel != null || model.ListCategoryViewModel.Count() !=0)
            {
                return View(model);
            }
            else
            {
                error.ErrorMessage = "Bạn cần phải tạo danh mục";
                return View("Error", error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductCrudModel model)
        {
            if(ModelState.IsValid)
            {
                var error = new ErrorViewModel();
                try
                {
                    var product = new Product();
                    product.Name = model.Name;
                    product.Code = model.Code;
                    product.Status = model.Status;
                    product.Quantity = model.Quantity;
                    product.Description = model.Description;
                    product.BasePrice = model.BasePrice;
                    product.SellPrice = model.SellPrice;

                    var lsCategory = new List<ProductCategory>();
                    if(model.SelectedCategories == null)
                        return View("Error", "Lỗi ko có loại sản phẩm");
                    foreach(var categoryid in model.SelectedCategories)
                    {
                        var ct = _uow.CategoryRepository.FirstOrDefault(x => x.CategoryId == categoryid);
                        lsCategory.Add(new ProductCategory
                        {
                            Category = _mapper.Map<Category>(ct),
                            Product = product
                        });
                    }
                    product.ProductCategories =  lsCategory;
                    _uow.ProductRepository.Add(product);
                    await _uow.CommitAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    error.ErrorMessage = ex.Message;
                    return View("Error", error);
                }
            }
            return View(model);
        }

        // dùng ajax chỉnh lại thành post
        [Display(Name = "Xóa sản phẩm")]
        public async Task<ActionResult> Delete(int id)
        {
            var error = new ErrorViewModel();
            var product = _uow.ProductRepository.FirstOrDefault(x => x.ProductId == id);
            if (product != null)
            {
                product.IsDeleted = true;
                product.UpdatedAt = DateTime.Now;
                await _uow.CommitAsync();
                return RedirectToAction("Index");
            }
            else
            {
                error.ErrorMessage = "Sản phẩm ko tồn tại.";
                return View("Error", error);
            }
        }

        [Display(Name = "Chỉnh sửa sản phẩm")]
        public ActionResult Edit(int id)
        {
            var error = new ErrorViewModel();
            var product = _uow.ProductRepository
                .BuildQuery(x => x.ProductId == id)
                .Include(x => x.ProductCategories)
                .ThenInclude(x=>x.Category)
                .FirstOrDefault();

            var categorySelected = product.ProductCategories
                .Select(x => _mapper.Map<CategoryViewModel>(x.Category))
                .ToList();
            var category = _uow.CategoryRepository.BuildQuery(x => !x.IsDeleted).Select(x => _mapper.Map<CategoryViewModel>(x)).ToList();
            if (category == null) 
                return View("Error", "Lỗi");

            var rs = _mapper.Map<ProductCrudModel>(product);
            rs.ListCategoryViewModel = category;
            rs.SelectedCategories = categorySelected.Select(x => x.CategoryId).ToArray();

            return View(rs);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ProductCrudModel model)
        {
            var error = new ErrorViewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    var product = _uow.ProductRepository
                        .BuildQuery(x => x.ProductId == model.ProductId)
                        .Include(x=>x.ProductCategories).FirstOrDefault();
                    if (product != null)
                    {
                        product.UpdatedAt = DateTime.Now;
                        product.Name = model.Name;
                        product.IsActive = model.IsActive;
                        product.IsDeleted = false;
                        product.Quantity = model.Quantity;
                        product.Description = model.Description;
                        product.BasePrice = model.BasePrice;
                        product.SellPrice = model.SellPrice;
                        product.Code = model.Code;
                        product.Status = model.Status;
                        var newCategory = model.SelectedCategories;

                        foreach (var item in product.ProductCategories)
                        {
                            product.ProductCategories.Remove(item);

                        }
                        foreach (var id in newCategory)
                        {
                            var category = _uow.CategoryRepository.FirstOrDefault(x => x.CategoryId == id);
                           
                            product.ProductCategories.Add(new ProductCategory() { 
                                Category = _mapper.Map<Category>(category),
                                Product = product 
                            });
                        }

                        await _uow.CommitAsync();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        error.ErrorMessage = "Lỗi không tìm thấy sản phẩm";
                        return View("Error", error);
                    }
                }
                catch
                {
                    error.ErrorMessage = "Lỗi khi chỉnh sửa sản phẩm";
                    return View("Error", error);
                }

            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> SetStatus(int productId)
        {
            var product = _uow.ProductRepository.FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                product.IsActive = !product.IsActive;
                product.UpdatedAt = DateTime.Now;
                await _uow.CommitAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
