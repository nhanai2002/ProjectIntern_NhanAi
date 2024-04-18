using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebShop.Permission;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.Repositories;
using WebShopCore.ViewModel.Auth;
using WebShopCore.ViewModel.User;

namespace WebShop.Controllers
{
    [Display(Name = "Người dùng")]
    [LoginRequired]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UserController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [Display(Name = "Xem danh sách người dùng")]
        public IActionResult Index()
        {
            var users = _uow.UserRepository
                .BuildQuery(x => !x.IsDeleted && x.RoleId == (int)SysEnum.DefaultRole.EndUser)
                .Include(x => x.Role)
                    .ThenInclude(x => x.ActionRole)
                    .ThenInclude(x => x.ActionCtrl)
                .Select(x => _mapper.Map<AuthenticationModel>(x))
                .ToList();
            return View(users);
        }

        [Display(Name = "Đổi mật khẩu người dùng")]
        public ActionResult ChangePassword(int userId)
        {
            var user = _uow.UserRepository.FirstOrDefault(x => x.UserId == userId);
            if (user != null)
            {
                var auth = new ChangePasswordViewModel
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Password = user.Password,
                };
                return View(auth);
            }
            return View("Error", "Khong tim thay user");
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _uow.UserRepository.FirstOrDefault(x => x.UserId == model.UserId);
                user.Password = model.Password.HasPassword();
                user.UpdatedAt = DateTime.Now;
                await _uow.CommitAsync();
                return RedirectToAction("Index");

            }
            return View(model);
        }

    }
}
