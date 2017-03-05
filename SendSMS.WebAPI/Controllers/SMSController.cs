using System;
using System.Threading.Tasks;
using System.Web.Http;
using SendSMS.WebAPI.BusinessLogic;
using SendSMS.WebAPI.Models;

namespace SendSMS.WebAPI.Controllers
{
    public class SMSController : ApiController
    {
        private readonly ISMSSender _smsSender;

        public SMSController(ISMSSender smsSender)
        {
            _smsSender = smsSender;
        }

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
        public async Task<Data.State> SendSMSAsync(string from, string to, string text)
        {
            return await DataManager.SendSMSAsync(from, to, text, _smsSender);
        }

        // GET: /sms/sent.json?dateTimeFrom=2015-03-01T11:30:20&dateTimeTo=2015-03-02T09:20:22&skip=100&take=50
        // GET: /sms/sent.xml?dateTimeFrom=2015-03-01T11:30:20&dateTimeTo=2015-03-02T09:20:22&skip=100&take=50
        /// <summary>
        /// Gets the SMS sent earlier.
        /// </summary>
        /// <param name="dateTimeFrom">The earliest date and time to look. UTC.</param>
        /// <param name="dateTimeTo">The latest date time to look. UTC.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="take">The number of records to take.</param>
        /// <returns>The suitable records and their total count.</returns>
        public async Task<GetSentSMSResponse> GetSentSMSAsync(DateTime? dateTimeFrom = null,
                                                              DateTime? dateTimeTo = null,
                                                              int skip = 0, int? take = null)
        {
            return await DataManager.GetSentSMSAsync(dateTimeFrom, dateTimeTo, skip, take);
        }
    }
}
