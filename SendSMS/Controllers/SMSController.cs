using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using SendSMS.Logic;
using SendSMS.Models;
using SendSMS.Models.API;

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
        public Models.DB.State SendSMS(string from, string to, string text)
        {
            Models.DB.Country country = SMSHelper.IdentifyCountry(to, _db.Countries);
            var state = Models.DB.State.Failed;
            if (country != null)
            {
                state = _sender.SendSMS(from, to, country.MobileCode, text);
            }
            var sms = new Models.DB.SMS(from, to, country, state);
            _db.SentSMS.Add(sms);
            _db.Entry(sms).State = EntityState.Added;
            _db.SaveChanges();
            return state;
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
        public GetSentSMSResult GetSentSMS(DateTime dateTimeFrom, DateTime dateTimeTo, int skip, int take)
        {
            List<Models.DB.SMS> records =
                SMSHelper.FilterSMS(dateTimeFrom, dateTimeTo, skip, take, _db.SentSMS).ToList();
            return new GetSentSMSResult
            {
                TotalCount = records.Count,
                Items = records.Select(SMS.FromDB)
            };
        }
    }
}
