using System;
using System.Collections.Generic;
using System.Web.Http;
using SendSMS.BusinessLogic;
using SendSMS.Models;

namespace SendSMS.Controllers
{
    public class StatisticsController : ApiController
    {
        // GET: statistics.json?dateFrom=2015-03-01&dateTo=2015-03-05&mccList=262,232
        // GET: statistics.xml?dateFrom=2015-03-01&dateTo=2015-03-05&mccList=262,232
        /// <summary>
        /// Gets the statistics for days and counties.
        /// </summary>
        /// <param name="dateFrom">The earliest date to look.</param>
        /// <param name="dateTo">The latest date to look.</param>
        /// <param name="mccList">A list of mobile country codes to filter, e.g. "262,232".
        /// If list is empty this means: include all mobile country codes.</param>
        public IEnumerable<Record> GetStatistics(DateTime? dateFrom = null, DateTime? dateTo = null,
                                                 string mccList = null)
        {
            return DataManager.GetStatistics(dateFrom, dateTo, mccList);
        }
    }
}
