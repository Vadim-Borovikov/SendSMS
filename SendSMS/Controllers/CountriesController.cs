using System.Linq;
using System.Web.Http;
using SendSMS.Models;

namespace SendSMS.Controllers
{
    public class CountriesController : ApiController
    {
        private readonly SendSMSContext _db = new SendSMSContext();

        // GET: api/Countries
        public Country[] GetCountries() => _db.Countries.ToArray();
    }
}