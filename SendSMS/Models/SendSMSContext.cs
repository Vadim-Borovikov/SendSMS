using System.Data.Entity;
using RichardLawley.EF.AttributeConfig;

namespace SendSMS.Models
{
    public class SendSMSContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<SMS> SentSMS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add conventions for Precision Attributes
            modelBuilder.Conventions.Add(new DecimalPrecisionAttributeConvention());
        }
    }
}
