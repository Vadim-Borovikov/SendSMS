using System.Data.Entity;

namespace SendSMS.Data
{
    public static class ContextHelper
    {
        public static void AddSMS(SMS sms, Context db)
        {
            db.SentSMS.Add(sms);
            db.Entry(sms).State = EntityState.Added;
            db.SaveChanges();
        }
    }
}
