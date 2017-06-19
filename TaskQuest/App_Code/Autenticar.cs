using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TaskQuest.App_Code
{
    public class Autenticar : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.User.Identity.IsAuthenticated;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.Path.EndsWith("Index", StringComparison.InvariantCultureIgnoreCase))
            {
                var httpContext = new HttpContextWrapper(HttpContext.Current);
                var newValues = new Dictionary<string, object> { { "Response", "Acesso inválido" } };
                new SessionStateTempDataProvider().SaveTempData(new ControllerContext { HttpContext = httpContext }, newValues);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Inicio" }));
            }
        }
    }
}