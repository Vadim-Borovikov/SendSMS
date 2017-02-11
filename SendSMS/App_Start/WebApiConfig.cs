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
                name: "Countries",
                routeTemplate: "countries.{ext}",
                defaults: new { controller = "countries" },
                constraints: new
                {
                    ext = new RegexRouteConstraint(ExtRegexPattern)
                }
            );

            config.Routes.MapHttpRoute(
                name: "Send SMS",
                routeTemplate: "sms/send.{ext}",
                defaults: new { controller = "sms", action = "SendSMS" },
                constraints: new
                {
                    ext = new RegexRouteConstraint(ExtRegexPattern)
                }
            );

            config.Routes.MapHttpRoute(
                name: "Sent SMS",
                routeTemplate: "sms/sent.{ext}",
                defaults: new { controller = "sms", action = "GetSentSMS" },
                constraints: new
                {
                    ext = new RegexRouteConstraint(ExtRegexPattern)
                }
            );

            config.Formatters.JsonFormatter.AddUriPathExtensionMapping("json", "application/json");
            config.Formatters.XmlFormatter.AddUriPathExtensionMapping("xml", "text/xml");
        }

        private const string ExtRegexPattern = "^xml$|^json$";
    }
}
