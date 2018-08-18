using System.Web;
using System.Web.Mvc;
using VibExchange.Filters;

namespace VibExchange
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new SessionExpireFilterAttribute());
        }
    }
}