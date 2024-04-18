using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebShopCore.Const;
using WebShopCore.Interfaces;
using WebShopCore.ViewModel.Auth;

namespace WebShop.Controllers
{
    public class StaffController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public StaffController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [Display(Name = "Xem danh sách nhân viên")]
        public IActionResult Index()
        {
            var users = _uow.UserRepository
                        .BuildQuery(x => !x.IsDeleted 
                                    && x.RoleId != (int)SysEnum.DefaultRole.EndUser 
                                    && x.RoleId != (int)SysEnum.DefaultRole.Admin)
                        .Include(x => x.Role)
                            .ThenInclude(x => x.ActionRole)
                            .ThenInclude(x => x.ActionCtrl)
                        .Select(x => _mapper.Map<AuthenticationModel>(x))
                        .ToList();
            return View(users);

        }
    }
}
