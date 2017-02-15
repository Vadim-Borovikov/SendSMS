using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SendSMS.WebAPI.Areas.HelpPage.ModelDescriptions;
using SendSMS.WebAPI.Areas.HelpPage.Models;

namespace SendSMS.WebAPI.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            Collection<ApiDescription> descriptions = Configuration.Services.GetApiExplorer().ApiDescriptions;
            descriptions.ForEach(ReplaceExtInPath);
            return View(descriptions);
        }

        public ActionResult Api(string apiId)
        {
            if (!string.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    RemoveExtUriParam(apiModel);
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!string.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }

        private static void ReplaceExtInPath(ApiDescription description)
        {
            description.RelativePath = description.RelativePath.Replace(".{ext}", ".(json|xml)");
        }

        private static void RemoveExtUriParam(HelpPageApiModel apiModel)
        {
            ParameterDescription uriParam = apiModel.UriParameters.FirstOrDefault(p => p.Name == "ext");
            if (uriParam != null)
            {
                apiModel.UriParameters.Remove(uriParam);
            }
        }
    }
}