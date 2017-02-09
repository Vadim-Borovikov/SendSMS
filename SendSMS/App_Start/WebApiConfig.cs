using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Routing.Constraints;

namespace SendSMS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Api UriPathExtension",
                routeTemplate: "{controller}.{ext}",
                defaults: new { },
                constraints: new
                {
                    ext = new RegexRouteConstraint("^xml$|^json$")
                }
            );

            config.Formatters.JsonFormatter.AddUriPathExtensionMapping("json", "application/json");
            config.Formatters.XmlFormatter.AddUriPathExtensionMapping("xml", "text/xml");
        }
    }
}
