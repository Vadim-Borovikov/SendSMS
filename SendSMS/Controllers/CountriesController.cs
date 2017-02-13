using System.Collections.Generic;
using System.Web.Http;
using SendSMS.BusinessLogic;
using SendSMS.Models;

namespace SendSMS.Controllers
{
    public class CountriesController : ApiController
    {
        private readonly Data.Context _db = new Data.Context();

        // GET: countries.json
        // GET: countries.xml
        /// <summary>
        /// Gets the countries list.
        /// </summary>
        public IEnumerable<Country> GetCountries() => DataManager.GetCountries(_db.Countries);
    }
}