using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace SendSMS.WebAPI.Models
{
    internal class DateTimeValueProvider : IValueProvider
    {
        private readonly HttpActionContext _actionContext;
        private readonly string _format;

        public DateTimeValueProvider(HttpActionContext actionContext, string format)
        {
            _actionContext = actionContext;
            _format = format;
        }

        public bool ContainsPrefix(string prefix) => false;

        public ValueProviderResult GetValue(string key)
        {
            string dateToParse = GetParameter(_actionContext, key);
            DateTime? dateTime = ParseDateTime(dateToParse, _format, key);
            return new ValueProviderResult(dateTime, null, CultureInfo.InvariantCulture);
        }

        private static string GetParameter(HttpActionContext actionContext, string name)
        {
            IEnumerable<KeyValuePair<string, string>> nameValuePairs = actionContext.Request.GetQueryNameValuePairs();
            return nameValuePairs.FirstOrDefault(q => q.Key.Equals(name)).Value;
        }

        private static DateTime? ParseDateTime(string dateToParse, string format, string parameterName)
        {
            if (dateToParse == null)
            {
                return null;
            }

            try
            {
                return DateTime.ParseExact(dateToParse, format, CultureInfo.InvariantCulture,
                                           DateTimeStyles.AdjustToUniversal);
            }
            catch (FormatException)
            {
                throw new FormatException($"Value \"{dateToParse}\" of URI parameter \"{parameterName}\" doesn't meet the expected format \"{format}\".");
            }
        }

    }
}