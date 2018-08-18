using System.Web.Mvc;

namespace VibExchange.Areas.RemoteAnalysis
{
    public class RemoteAnalysisAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RemoteAnalysis";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RemoteAnalysis_default",
                "RemoteAnalysis/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
