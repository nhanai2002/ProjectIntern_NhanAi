using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.Repositories;
using WebShopCore.Services.MailService;
using WebShopCore.ViewModel;
using WebShopCore.ViewModel.Auth;
using WebShopCore.ViewModel.User;

namespace WebShop.Controllers
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
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
                return RedirectToAction("Error", new { errorMessage = "Thông tin đăng nhập không hợp lệ" });
            }
            //if(user.RoleId == (int)SysEnum.DefaultRole.EndUser)
            //{
            //    return RedirectToAction("ProhibitAccess", "Home");
            //}
            HttpContext.Session.SetCurrentAuthentication(user);
            var lastRequestURL = HttpContext.Session.GetString(TextConstant.LastRequestURL);
            //if (lastRequestURL.Contains("/Home/ProhibitAccess"))
            //{
            //    return Redirect("/");
            //}
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
                && x.RoleId != (int) SysEnum.DefaultRole.EndUser)
                .Include(x => x.Role)
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

        public ActionResult Logout()
        {
            HttpContext.Session.RemoveSession();
            return RedirectToAction("Login", "Auth");
        }


    }
}
