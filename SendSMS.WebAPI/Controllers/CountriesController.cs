using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using SendSMS.WebAPI.BusinessLogic;
using SendSMS.WebAPI.Models;

namespace SendSMS.WebAPI.Controllers
{
    public class CountriesController : ApiController
    {
        // GET: countries.json
        // GET: countries.xml
        /// <summary>
        /// Gets the countries list.
        /// </summary>
        public async Task<List<Country>> GetCountriesAsync() => await DataManager.GetCountriesAsync();
    }
}