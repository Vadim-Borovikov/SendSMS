using System.Collections.Generic;
using System.Web.Http;
using SendSMS.BusinessLogic;
using SendSMS.Models;

namespace SendSMS.WebAPI.Controllers
{
    public class CountriesController : ApiController
    {
        // GET: countries.json
        // GET: countries.xml
        /// <summary>
        /// Gets the countries list.
        /// </summary>
        public IEnumerable<Country> GetCountries() => DataManager.GetCountries();
    }
}