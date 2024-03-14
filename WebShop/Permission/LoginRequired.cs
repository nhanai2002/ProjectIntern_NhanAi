using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Interfaces;
using WebShopCore.Repositories;

namespace WebShop.Permission
{
    public class LoginRequired : ActionFilterAttribute
    {
        public LoginRequired() 
        {
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var _uow = filterContext.HttpContext.RequestServices.GetService<IUnitOfWork>();

            base.OnActionExecuting(filterContext);

            HttpContext context = filterContext.HttpContext;
            HttpRequest request = filterContext.HttpContext.Request;

            var currentAuthentication = context.Session.GetCurrentAuthentication();
            if (currentAuthentication == null)
            {
                if (request.Method == "GET")
                {
                    context.Session.SetString(TextConstant.LastRequestURL, request.GetDisplayUrl());
                }
                filterContext.Result = new RedirectResult("/Auth/Login");
                return;
            }

            // kiểm tra role
            if (currentAuthentication.RoleId != null) 
            {
               var user = _uow.UserRepository
                    .BuildQuery(x => x.UserId == currentAuthentication.UserId)
                    .Include(x => x.Role)
                    .ThenInclude(x => x.ActionRole)
                    .ThenInclude(x => x.ActionCtrl)
                    .FirstOrDefault();
               
                if (user != null)
                {
                    if(user.RoleId == 1)
                    {
                        return;
                    }    
                    string currentCtrl = filterContext.RouteData.Values["controller"].ToString();
                    string currentAction = filterContext.RouteData.Values["action"].ToString();
                    string keyValue = $"{currentCtrl}.{currentAction}";

                    var role = user.Role.ActionRole.Where(x => x.ActionCtrl.ActionName.Equals(keyValue));
                    if (role.Any())
                    {
                        return;
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("/Home/Index");
                        return;
                    }
                    
                }
            }
        }


    }
}
