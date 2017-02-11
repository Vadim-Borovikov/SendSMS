using System.Web.Http;
using SendSMS.Logic;
using SendSMS.Models;

namespace SendSMS.Controllers
{
    public class SMSController : ApiController
    {
        private readonly SendSMSContext _db = new SendSMSContext();
        private readonly ISMSSender _sender = new DummySMSSender();

        // GET: sms/send.json?from=The+Sender&to=%2B4917421293388&text=Hello+World
        // GET: sms/send.xml?from=The+Sender&to=%2B4917421293388&text=Hello+World
        /// <summary>
        /// Sends the SMS.
        /// </summary>
        /// <param name="from">The sender of the message.</param>
        /// <param name="to">The receiver of the message. Number that starts with + and country code.</param>
        /// <param name="text">The text which should be sent.</param>
        /// <returns>The SMS sending state.</returns>
        [HttpGet]
        public State SendSMS(string from, string to, string text)
        {
            Country country = SMSHelper.IdentifyCountry(to, _db.Countries);
            var state = State.Failed;
            if (country != null)
            {
                state = _sender.SendSMS(from, to, country.MobileCode, text);
            }
            var sms = new SMS(from, to, country, state);
            _db.SentSMS.Add(sms);
            _db.SaveChanges();
            return state;
        }
    }
}
