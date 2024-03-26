using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.Services.MailService;
using WebShopCore.ViewModel.Auth;
using WebShopCore.ViewModel.User;
using WebShopCore.ViewModel;
using Microsoft.EntityFrameworkCore;
using WebShopEndUser.Models;
using WebShopCore.Repositories;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;

namespace WebShopEndUser.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ISendMailService _sendMail;
        public AuthController(IUnitOfWork uow, IMapper mapper, ISendMailService sendMail)
        {
            _uow = uow;
            _mapper = mapper;
            _sendMail = sendMail;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = new AuthenticationModel();
            if (model.Password != null)
            {
                model.Password = model.Password.HasPassword();
            }
            try
            {
                user = LoginValid(model);
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
                return View(model);
            }

            if (user == null)
            {
                return RedirectToAction("Error", new { rrorMessage = "Thông tin đăng nhập không hợp lệ" });
            }

            HttpContext.Session.SetCurrentAuthentication(user);
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

        private AuthenticationModel LoginValid(LoginViewModel model)
        {
            var user = _uow.UserRepository.BuildQuery(
                x => x.UserName == model.UserName
                && x.Password == model.Password
                && x.IsActive
                && !x.IsDeleted
                ).Include(x => x.Role)
                .ThenInclude(x => x.ActionRole)
                .ThenInclude(x => x.ActionCtrl)
                .FirstOrDefault();

            if (user == null)
            {
                throw new Exception("Đăng nhập ko hợp lệ");
            }
            if (user.IsActive == false)
            {
                throw new Exception("Tài khoản đang bị khóa");
            }
            var authenticationModel = CreateAuthModel(user);
            return authenticationModel;
        }

        private AuthenticationModel CreateAuthModel(User user)
        {
            var authenticationModel = new AuthenticationModel()
            {
                Name = user.Name,
                UserGuid = user.UserGuid,
                UserId = user.UserId,
                RoleId = user.RoleId,
                AvatarUrl = string.IsNullOrEmpty(user.Avatar) ? "/img/baseAvatar.jpg" : user.Avatar,
                Email = user.Email,
                UserName = user.UserName,
                Phone = user.Phone,
            };
            return authenticationModel;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.RemoveSession();
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserCrudModel model)
        {
            var errorModel = new ErrorViewModel();
            try
            {
                if (model.UserName != null)
                {
                    var checkExistUser = _uow.UserRepository
                        .FirstOrDefault(x => x.UserName!.Trim().ToLower() == model.UserName.Trim().ToLower() && !x.IsDeleted);
                    if (checkExistUser != null)
                    {
                        throw new Exception("Tài khoản này đã tồn tại!");
                    }
                }
                if (ModelState.IsValid)
                {
                    User user = new()
                    {
                        UserName = model.UserName,
                        Email = model.Email!.Trim().ToLower(),
                        UserGuid = Guid.NewGuid(),
                        Name = model.Name!.Trim(),
                        BirthDay = model.BirthDay,
                        Gender = model.Gender,
                        Phone = model.Phone,
                        Address = model.Phone,
                        Password = model.Password!.HasPassword(),
                        RoleId = (int)SysEnum.DefaultRole.EndUser,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };
                    _uow.UserRepository.Add(user);
                    await _uow.CommitAsync();
                    var sendMail = SendToEmailUser(user.UserId, "ConfirmEmail", "xác nhận email");
                    if (sendMail == true)
                    {
                        return View("CheckMail", "Auth");
                    }
                    return RedirectToAction("Login");

                }
                else
                {
                    model.ErrorMessage = "Lỗi khi tạo tài khoản";
                    return View(model);
                }
            }
            catch (Exception ex)
            { 
                model.ErrorMessage = ex.Message;
                var baseException = ex.GetBaseException();
                model.ErrorMessage = "Lỗi khi tạo tài khoản";
                return View(model);

            }

        }

        public async Task<IActionResult> ConfirmEmail(string username, string code)
        {
            if (code == null) return NotFound();
            var bytes = WebEncoders.Base64UrlDecode(code);

            var decoded = new Guid(bytes);
            var u = _uow.UserRepository.FirstOrDefault(x => x.UserName == username);
            if (u == null) return NotFound();

            if (decoded.ToString() == u.TokenEmail)
            {
                u.ConfirmEmail = true;
                await _uow.CommitAsync();
            }
            return RedirectToAction("Login", "Auth");
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ChangePasswordViewModel model)
        {
            var checkExistUser = _uow.UserRepository.FirstOrDefault(x => x.UserName == model.UserName && x.Email == model.Email && x.ConfirmEmail == true);
            if (checkExistUser == null)
            {
                return View("Error", "Tài khoản không tồn tại");
            }

            var sendMail = SendToEmailUser(checkExistUser.UserId, "ResetPassword", "đổi mật khẩu");
            if (sendMail == true)
            {
                return View("CheckMail", "Auth");
            }
            else
            {
                return View(model);
            }
        }


        private bool SendToEmailUser(int userId, string actionName, string message)
        {
            try
            {
                var checkExistUser = _uow.UserRepository.FirstOrDefault(x => x.UserId == userId);
                var code = Guid.NewGuid();
                var bytes = code.ToByteArray();
                var encoded = WebEncoders.Base64UrlEncode(bytes);

                var callbackUrl = Url.ActionLink(
                   action: actionName,
                    values:
                        new
                        {
                            username = checkExistUser.UserName,
                            code = encoded
                        },
                    protocol: Request.Scheme);

                checkExistUser.TokenEmail = code.ToString();
                _uow.Commit();

                var send = new MailContent
                {
                    To = checkExistUser.Email,
                    Subject = "Xác nhận địa chỉ email",
                    Body = @$"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>bấm vào đây để {message}</a>."
                };

                _sendMail.SendMail(send).Wait();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public ActionResult CheckMail()
        {
            return View();
        }

        public ActionResult ResetPassword(string username, string code)
        {
            var user = _uow.UserRepository.FirstOrDefault(x => x.UserName == username);
            if (user != null)
            {
                var bytes = WebEncoders.Base64UrlDecode(code);
                var decoded = new Guid(bytes);
                if (decoded.ToString() == user.TokenEmail)
                {
                    var reset = new ChangePasswordViewModel();
                    reset.UserName = username;
                    return View(reset);
                }
            }
            return RedirectToAction("ForgotPassword", "Auth");
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _uow.UserRepository.FirstOrDefault(x => x.UserName == model.UserName);
                if (user != null)
                {
                    user.Password = model.Password.HasPassword();
                    await _uow.CommitAsync();
                    return RedirectToAction("Login", "Auth");
                }
            }

            return View(model);
        }


    }
}
