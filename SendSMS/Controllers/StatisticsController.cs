using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SendSMS.Logic;
using SendSMS.Models;
using SendSMS.Models.API;

namespace SendSMS.Controllers
{
    public class StatisticsController : ApiController
    {
        private readonly SendSMSContext _db = new SendSMSContext();

        // GET: statistics.json?dateFrom=2015-03-01&dateTo=2015-03-05&mccList=262,232
        // GET: statistics.xml?dateFrom=2015-03-01&dateTo=2015-03-05&mccList=262,232
        /// <summary>
        /// Gets the statistics for days and counties.
        /// </summary>
        /// <param name="dateFrom">The earliest date to look.</param>
        /// <param name="dateTo">The latest date to look.</param>
        /// <param name="mccList">A list of mobile country codes to filter, e.g. "262,232".
        /// If list is empty this means: include all mobile country codes.</param>
        public IEnumerable<Record> GetStatistics(DateTime dateFrom, DateTime dateTo, string mccList)
        {
            short[] codes = mccList.Split(',').Select(short.Parse).ToArray();
            return SMSHelper.GetStatistics(dateFrom, dateTo, codes, _db.Countries, _db.SentSMS);
        }
    }
}
