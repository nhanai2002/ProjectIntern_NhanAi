using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebShop.Models;
using WebShop.Permission;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.Repositories;
using WebShopCore.ViewModel.Category;

namespace WebShop.Controllers
{
    [Display(Name = "Thể loại")]
    [LoginRequired]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [Display(Name = "Xem thể loại")]
        public IActionResult Index()
        {
            var data = _uow.CategoryRepository
                .BuildQuery(x => !x.IsDeleted)
                .Select(x => _mapper.Map<CategoryViewModel>(x));
            return View(data);
        }

        [Display(Name = "Thêm thể loại")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryCrudModel model)
        {
            if(ModelState.IsValid)
            {
                var error = new ErrorViewModel();
                try
                {
                    model.IsActive = true;
                    _uow.CategoryRepository.Add(_mapper.Map<Category>(model));
                    await _uow.CommitAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    error.ErrorMessage = ex.Message;
                    return View("Error", error);
                }
            }
            else {
                return View(model);
            }
        }

        [Display(Name = "Xóa thể loại")]
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var category = _uow.CategoryRepository.FirstOrDefault(x => x.CategoryId == id);
            var check = _uow.ProductCategoryRepository.BuildQuery(x => x.CategoryId == category.CategoryId).ToList();
            if(check.Any() || check.Count() > 0)
            {
                return Json(new { success = false , message = "hasproduct"});
            }
            if (category != null)
            {
                category.IsDeleted = true;
                category.UpdatedAt = DateTime.Now;
                await _uow.CommitAsync();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "fail" });
            }
        }

        [Display(Name = "Chỉnh sửa thể loại")]
        public ActionResult Edit(int id)
        {
            var category = _uow.CategoryRepository.FirstOrDefault(x => x.CategoryId == id);
            var data = _mapper.Map<CategoryCrudModel>(category);
            if (data == null) return View("Error", "Lỗi ko tìm thấy thể loại này");
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CategoryCrudModel model)
        {
            if(ModelState.IsValid)
            {
                var error = new ErrorViewModel();
                try
                {
                    var category = _uow.CategoryRepository.FirstOrDefault(x => x.CategoryId == model.CategoryId);
                    if (category != null)
                    {
                        category.UpdatedAt = DateTime.Now;
                        category.IsActive = true;
                        category.Name = model.Name;
                        await _uow.CommitAsync();
                        return RedirectToAction("Index");
                    }
                    else {
                        error.ErrorMessage = "Lỗi ko tìm thấy category";
                        return View("Error", error); 
                    }
                }
                catch (Exception e)
                {
                    error.ErrorMessage = e.Message;
                    return View("Error", error);
                }

            }
            return View(model);
        }
    }
}
