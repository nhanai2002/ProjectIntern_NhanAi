using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Interfaces;
using WebShopCore.Repositories;

namespace WebShopEndUser.Permission
{
    public class LoginRequired : ActionFilterAttribute, IActionFilter
    {
        public LoginRequired() 
        {
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var _uow = filterContext.HttpContext.RequestServices.GetService<IUnitOfWork>() as UnitOfWork;

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

        }


    }
}
