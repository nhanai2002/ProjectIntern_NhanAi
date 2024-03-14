using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WebShop.Models;
using WebShop.Permission;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.Repositories;
using WebShopCore.ViewModel.Auth;
using WebShopCore.ViewModel.UserRole;

namespace WebShop.Controllers
{
    [Display(Name = "Vai trò")]
    [LoginRequired]
    public class UserRoleController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        public UserRoleController(IUnitOfWork uow, IMapper mapper, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _uow = uow; ;
            _mapper = mapper;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }
        
        public IActionResult Index()
        {
            var data = _uow.RoleRepository
                .BuildQuery(x => !x.IsDeleted && x.IsActive)
                .Select(x => _mapper.Map<RoleViewModel>(x));
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoleCrudModel model)
        {
            if (ModelState.IsValid)
            {
                var error = new ErrorViewModel();
                try
                {
                    model.IsActive = true;
                    _uow.RoleRepository.Add(_mapper.Map<WebShopCore.Model.Role>(model));
                    await _uow.CommitAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    var e = ex.GetBaseException();
                    error.ErrorMessage = ex.Message;
                    return View("Error", error);
                }
            }
            return View(model);
        }


        // kiểm tra xem nếu có user nào có vai trò đó thì không đc xóa
        public async Task<ActionResult> Delete(int id)
        {
            var role = _uow.RoleRepository.FirstOrDefault(x => x.RoleId == id);
            if(role != null)
            {
                role.IsDeleted = true;
                role.UpdatedAt = DateTime.Now;
                await _uow.CommitAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", "Lỗi khi xóa");
            }
        }


       
        public IActionResult SetActionForRole(int RoleId)
        {
            var roleFormDb = _uow.RoleRepository
                .BuildQuery(x => x.RoleId == RoleId && !x.IsDeleted && x.IsActive)
                .Include(x => x.ActionRole)
                .ThenInclude(x => x.ActionCtrl)
                .FirstOrDefault();

            var role = new RoleCrudModel();
            role.RoleId = roleFormDb.RoleId;
            role.Name = roleFormDb.Name;
            role.ctrls = GetControllerAndAction(roleFormDb.ActionRole.ToList());

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> SetActionForRole(RoleCrudModel model)
        {
            var error = new ErrorViewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    var actionRoles = _uow.ActionRoleRepository.BuildQuery(x => x.RoleId == model.RoleId).ToList();
                    if(actionRoles.Any() && actionRoles != null)
                    {
                        foreach(var item in actionRoles)
                        {
                            _uow.ActionRoleRepository.Delete(item);
                        }
                        await _uow.CommitAsync();
                    }
                    var actions = new List<ActionCtrl>();
                    foreach (var ActionName in model.SelectedActions)
                    {
                        var action = new ActionCtrl
                        {
                            ActionName = ActionName
                        };
                        _uow.ActionCtrlRepository.Add(action);
                        await _uow.CommitAsync();

                        var actionRole = new ActionRole
                        {
                            ActionCtrlId = action.ActionCtrlId,
                            RoleId = model.RoleId
                        };
                        _uow.ActionRoleRepository.Add(actionRole);
                    }
                    await _uow.CommitAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    error.ErrorMessage = ex.GetBaseException().ToString();
                    return View("Error", error);
                }
            }
            error.ErrorMessage = "Lỗi set role";
            return RedirectToAction("Error", error);
        }


        // set role cho user
        public ActionResult SetRole(int UserId)     
        {
            var error = new ErrorViewModel();
            var roleFormDb = _uow.RoleRepository
                        .BuildQuery(x =>!x.IsDeleted && x.IsActive).ToList();
            var roles = _mapper.Map<List<RoleViewModel>>(roleFormDb);

            var user = _uow.UserRepository
                .BuildQuery(x => x.UserId == UserId && !x.IsDeleted)
                .Include(x => x.Role)
                .ThenInclude(x => x.ActionRole)
                .ThenInclude(x => x.ActionCtrl)
                 .Select(x => _mapper.Map<AuthenticationModel>(x))
                .FirstOrDefault();
            if (user == null || roles == null) {
                error.ErrorMessage = "Lỗi không thể set role";
                return View("Error", error);
            }

            var rs = new SetRoleViewModel();
            rs.UserId = UserId;
            rs.Username = user.UserName;
            rs.Roles = roles;
            
            return View(rs);
        }

        [HttpPost]
        public async Task<ActionResult> SetRole(SetRoleViewModel model)
        {
            var error = new ErrorViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _uow.UserRepository.FirstOrDefault(x => x.UserId == model.UserId && x.UserName == model.Username);
                    if (user != null)
                    {
                        user.RoleId = model.RoleId;
                        await _uow.CommitAsync();
                        return RedirectToAction("Index", "User");
                    }
                }
                else
                {
                    error.ErrorMessage = "Lỗi không thể set role";
                    return View("Error", error);
                }
            }
            catch (Exception ex)
            {
                error.ErrorMessage = "Lỗi không thể set role";
                return View("Error", error);
            }
            return View(model);
        }


        private List<ControllerInfo> GetControllerAndAction(List<ActionRole> model)
        {
            var controllers = new List<ControllerInfo>();

            List<string> ctrls = new();
            List<string> actions = new();

            foreach (var actionName in model)
            {
                var name = actionName.ActionCtrl.ActionName.Split(".");
                ctrls.Add(name[0]);
                actions.Add(name[1]);
            }

            var addedActions = new HashSet<string>();

            controllers = _actionDescriptorCollectionProvider
                .ActionDescriptors
                .Items
                .OfType<ControllerActionDescriptor>()
                .Where(a => a.ControllerName != "Home" && a.ControllerName != "Auth")
                .Where(a =>
                {
                    var actionKey = $"{a.ControllerName}.{a.ActionName}";
                    if (addedActions.Contains(actionKey))
                    {
                        // This action has already been added, so we skip it
                        return false;
                    }
                    else
                    {
                        // This action has not been added yet, so we add it to the set and include it in the list
                        addedActions.Add(actionKey);
                        return true;
                    }
                })
                .GroupBy(a => a.ControllerName)
                .Select(g => new ControllerInfo
                {
                    ControllerName = g.Key,
                    DisplayName = g.First().ControllerTypeInfo.GetCustomAttribute<DisplayAttribute>()?.Name ?? g.Key,
                    Actions = g.Select(a => new ActionInfo
                    {
                        ActionName = a.ActionName,
                        DisplayName = a.MethodInfo.GetCustomAttribute<DisplayAttribute>()?.Name ?? a.ActionName,
                        isChecked = ctrls.Contains(a.ControllerName) && actions.Contains(a.ActionName)
                    }).ToList()
                })
                .ToList();

            return controllers;
        }
    }
}
