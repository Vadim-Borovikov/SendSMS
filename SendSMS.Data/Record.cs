using System;

namespace SendSMS.Data
{
    /// <summary>
    /// The statistics record
    /// </summary>
    public class Record
    {
        public DateTime Day { get; set; }

        public Country Country { get; set; }

        public int Count;
    }
}
