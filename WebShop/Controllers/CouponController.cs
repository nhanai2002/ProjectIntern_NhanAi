using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebShop.Models;
using WebShop.Permission;
using WebShopCore.Const;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.Repositories;
using WebShopCore.Services.FileUploadService;
using WebShopCore.ViewModel.Coupon;

namespace WebShop.Controllers
{
    [LoginRequired]
    [Display(Name = "Coupon")]
    public class CouponController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CouponController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [Display(Name = "Xem coupon")]
        public IActionResult Index(string keyword, int? selectType)
        {
            var data = _uow.CouponRepository.BuildQuery(
                    x => !x.IsDeleted);
            if (keyword != null && keyword != "")
            {
                data = data.Where(x =>
                   EF.Functions.Like(x.Code!, $"%{keyword}%")
                   || EF.Functions.Like(x.Name!, $"%{keyword}%")
               );
            }

            switch (selectType)
            {
                case (int)SysEnum.CouponStatus.Happening:
                    data = data.Where(x => x.TimeStart.Date <= DateTime.Now.Date && x.TimeEnd.Date >= DateTime.Now.Date);
                    break;
                case (int)SysEnum.CouponStatus.End:
                    data = data.Where(x => x.TimeEnd.Date <= DateTime.Now.Date);
                    break;
                default:
                    break;
            }

            data = data.OrderByDescending(x => x.CreatedAt);
            var result = _mapper.Map<List<CouponViewModel>>(data.ToList());
            return View(result);
        }

        [Display(Name = "Thêm coupon")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CouponCrudModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.IsActive = true;
                    _uow.CouponRepository.Add(_mapper.Map<Coupon>(model));
                    await _uow.CommitAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View("Error", ex.Message);
                }
            }
            return View(model);
        }
        [Display(Name = "Xóa coupon")]
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var coupon = _uow.CouponRepository.FirstOrDefault(x => x.Id == id);
            if (coupon != null)
            {
                coupon.IsDeleted = true;
                coupon.UpdatedAt = DateTime.Now;
                await _uow.CommitAsync();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Lỗi khi xóa" });
            }
        }

        [Display(Name = "Chỉnh sửa coupon")]
        public ActionResult Edit(int id)
        {
            var coupon = _uow.CouponRepository.FirstOrDefault(x => x.Id == id);
            if (coupon == null)
            {
                var errorModel = new ErrorViewModel();
                errorModel.ErrorMessage = "Lỗi không tìm thấy coupon";
                return View("Error", errorModel);
            }
            var rs = _mapper.Map<CouponCrudModel>(coupon);
            return View(rs);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CouponCrudModel model)
        {
            if(ModelState.IsValid)
            {
                var error = new ErrorViewModel();
                try
                {
                    var coupon = _uow.CouponRepository.FirstOrDefault(x => x.Id == model.Id);
                    if(coupon != null)
                    {
                        coupon.UpdatedAt = DateTime.Now;
                        coupon.Name = model.Name;
                        coupon.Code = model.Code;
                        coupon.LimitationTimes = model.LimitationTimes;
                        coupon.CouponPriceType = model.CouponPriceType;
                        coupon.CouponPriceValue = model.CouponPriceValue;
                        coupon.TimeStart = model.TimeStart;
                        coupon.TimeEnd = model.TimeEnd;
                        await _uow.CommitAsync();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        error.ErrorMessage = "Lỗi ko tìm thấy mã khuyến mãi";
                        return View("Error", error);
                    }
                }
                catch(Exception ex)
                {
                    error.ErrorMessage = ex.Message;
                    return View("Error", error);
                }
            }
            return View(model);
        }


        [Display(Name = "Thay đổi trạng thái coupon")]
        [HttpPost]
        public async Task<ActionResult> SetStatus(int id)
        {
            var coupon = _uow.CouponRepository.FirstOrDefault(x => x.Id == id);
            if(coupon != null)
            {
                coupon.UpdatedAt = DateTime.Now;
                coupon.IsActive = !coupon.IsActive;
                await _uow.CommitAsync();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }


    }
}
