using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Interfaces;
using WebShopCore.Services.FileUploadService;
using WebShopCore.Services.MailService;
using WebShopCore.ViewModel.Auth;
using WebShopCore.ViewModel.User;

namespace WebShop.Controllers
{
    public class SettingController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        public SettingController(IUnitOfWork uow, IMapper mapper, ISendMailService sendMail, IFileUploadService fileUploadService)
        {
            _uow = uow;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
        }

        public IActionResult UserProfile()
        {
            var userCurrent = HttpContext.Session.GetCurrentAuthentication();
            var user = _uow.UserRepository.FirstOrDefault(x => x.UserName == userCurrent.UserName);
            if (user != null)
            {
                var rs = _mapper.Map<UserCrudModel>(user);
                return View(rs);
            }
            else
            {
                var lastRequestURL = HttpContext.Session.GetString(TextConstant.LastRequestURL);

                if (string.IsNullOrEmpty(lastRequestURL))
                {
                    return Redirect("/");
                }
                else
                {
                    return Redirect(lastRequestURL);
                }

            }
        }

        [HttpPost]
        public async Task<IActionResult> UserProfile(UserCrudModel model)
        {
            try
            {
                var user = _uow.UserRepository.FirstOrDefault(x => x.UserId == model.UserId);
                if (ModelState.IsValid)
                {
                    if (user != null)
                    {
                        user.Name = model.UserName;
                        user.Phone = model.Phone;
                        user.Address = model.Address;
                        user.Gender = model.Gender;
                        await _uow.CommitAsync();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
                return View(model);

            }
        }

        // tự đổi mật khẩu
        public IActionResult ChangePassword()
        {
            var getUser = HttpContext.Session.GetCurrentAuthentication(); 
            var user = _uow.UserRepository.FirstOrDefault(x => x.UserId == getUser.UserId);
            if (user != null)
            {
                var model = new ChangePasswordViewModel()
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                };
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var user = _uow.UserRepository.FirstOrDefault(x => x.UserId == model.UserId);
                if (ModelState.IsValid)
                {
                    if (user != null)
                    {
                        user.Password = model.Password.HasPassword();
                        user.UpdatedAt = DateTime.Now;
                        await _uow.CommitAsync();
                        HttpContext.Session.RemoveSession();
                        return RedirectToAction("Login", "Auth");
                    }
                    else
                    {
                        return View("Error", "Không tồn tại user");
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // đổi avatar
        public IActionResult UpdateAvatar()
        {
            var getUser = HttpContext.Session.GetCurrentAuthentication();
            var user = _uow.UserRepository.FirstOrDefault(x => x.UserId == getUser.UserId);
            if (user != null)
            {
                var result = _mapper.Map<UserCrudModel>(user);
                return View(result);
            }
            else
            {
                return View("Error", "Lỗi không tìm thấy user");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAvatar(UserCrudModel model)
        {
            var modelError = new ErrorViewModel();
            try
            {
                var user = _uow.UserRepository.FirstOrDefault(x => x.UserId == model.UserId);
                if (user != null)
                {
                    var url = await _fileUploadService.UpLoadFileAsync(model.FileImage);
                    user.Avatar = url;
                    await _uow.CommitAsync();
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

    }
}
