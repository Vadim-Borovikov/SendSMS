using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SendSMS.Models;

namespace SendSMS.Controllers
{
    public class CountriesController : ApiController
    {
        private readonly SendSMSContext _db = new SendSMSContext();

        // GET: api/countries
        public IEnumerable<CountryData> GetCountries() => _db.Countries.Select(CountryData.FromCountry);
    }
}