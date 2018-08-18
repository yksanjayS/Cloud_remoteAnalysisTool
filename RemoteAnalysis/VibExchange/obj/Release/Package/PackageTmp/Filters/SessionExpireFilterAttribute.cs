using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;


namespace VibExchange.Filters
{
    //public class LoggingFilterAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        filterContext.HttpContext.Trace.Write("(Logging Filter)Action Executing: " +
    //            filterContext.ActionDescriptor.ActionName);

    //        base.OnActionExecuting(filterContext);
    //    }

    //    public override void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //        if (filterContext.Exception != null)
    //            filterContext.HttpContext.Trace.Write("(Logging Filter)Exception thrown");

    //        base.OnActionExecuted(filterContext);
    //    }
    //}

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session.IsNewSession)
            {
                //Redirect
                
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content("~/Home/Login");
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }

        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
          string UserName =  filterContext.HttpContext.Session.Keys[1];
            string Username = ctx.User.Identity.Name;
            // If the browser session or authentication session has expired...
            if (ctx.Session["UserName"] == "" || !filterContext.HttpContext.Request.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    // For AJAX requests, we're overriding the returned JSON result with a simple string,
                    // indicating to the calling JavaScript code that a redirect should be performed.
                    filterContext.Result = new JsonResult { Data = "_Logon_" };
                }
                else
                {
                    // For round-trip posts, we're forcing a redirect to Home/TimeoutRedirect/, which
                    // simply displays a temporary 5 second notification that they have timed out, and
                    // will, in turn, redirect to the logon page.
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "Controller", "Home" },
                        { "Action", "Index" }
                });
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    //public class LocsAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    //{
    //    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    //    {
    //        HttpContext ctx = HttpContext.Current;

    //        // If the browser session has expired...
    //        if (ctx.Session["UserName"] == null)
    //        {
    //            if (filterContext.HttpContext.Request.IsAjaxRequest())
    //            {
    //                // For AJAX requests, we're overriding the returned JSON result with a simple string,
    //                // indicating to the calling JavaScript code that a redirect should be performed.
    //                filterContext.Result = new JsonResult { Data = "_Logon_" };
    //            }
    //            else
    //            {
    //                // For round-trip posts, we're forcing a redirect to Home/TimeoutRedirect/, which
    //                // simply displays a temporary 5 second notification that they have timed out, and
    //                // will, in turn, redirect to the logon page.
    //                filterContext.Result = new RedirectToRouteResult(
    //                    new RouteValueDictionary {
    //                    { "Controller", "Home" },
    //                    { "Action", "TimeoutRedirect" }
    //            });
    //            }
    //        }
    //        else if (filterContext.HttpContext.Request.IsAuthenticated)
    //        {
    //            // Otherwise the reason we got here was because the user didn't have access rights to the
    //            // operation, and a 403 should be returned.
    //            filterContext.Result = new HttpStatusCodeResult(403);
    //        }
    //        else
    //        {
    //            base.HandleUnauthorizedRequest(filterContext);
    //        }
    //    }
    //}
}