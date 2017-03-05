using System.Data.Entity;

namespace SendSMS.Data
{
    public class ContextInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context db)
        {
            db.Countries.Add(new Country { Name = "Germany", MobileCode = 262, Code = 49, PricePerSMS = 0.055m });
            db.Countries.Add(new Country { Name = "Austria", MobileCode = 232, Code = 43, PricePerSMS = 0.053m });
            db.Countries.Add(new Country { Name = "Poland", MobileCode = 260, Code = 48, PricePerSMS = 0.032m });

            db.Database.ExecuteSqlCommand(@"Create FUNCTION TruncateTime(dateValue DateTime) RETURNS date
                                          return Date(dateValue);");

            base.Seed(db);
        }
    }
}
