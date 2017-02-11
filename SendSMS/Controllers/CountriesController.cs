using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SendSMS.Models;
using SendSMS.Models.API;

namespace SendSMS.Controllers
{
    public class CountriesController : ApiController
    {
        private readonly SendSMSContext _db = new SendSMSContext();

        // GET: countries.json
        // GET: countries.xml
        /// <summary>
        /// Gets the countries list.
        /// </summary>
        public IEnumerable<Country> GetCountries() => _db.Countries.Select(Country.FromDB);
    }
}