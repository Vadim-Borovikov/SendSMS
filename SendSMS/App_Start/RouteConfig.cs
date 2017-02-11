using System.Web.Mvc;
using System.Web.Routing;

namespace SendSMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // By default route the user to the Help area if accessing the base URI.
            routes.MapRoute(
                name: "Help Area",
                url: "",
                defaults: new { controller = "Help", action = "Index" }
            ).DataTokens = new RouteValueDictionary(new { area = "HelpPage" });
        }
    }
}
