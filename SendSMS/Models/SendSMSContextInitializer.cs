using System.Data.Entity;
using SendSMS.Models.DB;

namespace SendSMS.Models
{
    public class SendSMSContextInitializer : DropCreateDatabaseIfModelChanges<SendSMSContext>
    {
        protected override void Seed(SendSMSContext db)
        {
            db.Countries.Add(new Country { Name = "Germany", MobileCode = 262, Code = 49, PricePerSMS = 0.055m });
            db.Countries.Add(new Country { Name = "Austria", MobileCode = 232, Code = 43, PricePerSMS = 0.053m });
            db.Countries.Add(new Country { Name = "Poland", MobileCode = 260, Code = 48, PricePerSMS = 0.032m });

            base.Seed(db);
        }
    }
}
