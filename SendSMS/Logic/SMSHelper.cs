using System;
using System.Collections.Generic;
using System.Linq;
using SendSMS.Models;

namespace SendSMS.Logic
{
    internal class SMSHelper
    {
        public static Country IdentifyCountry(string number, IEnumerable<Country> countries)
        {
            return countries.FirstOrDefault(c => number.StartsWith($"+{c.Code}", StringComparison.Ordinal));
        }
    }
}