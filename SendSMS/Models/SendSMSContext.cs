using System.Data.Entity;

namespace SendSMS.Models
{
    public class SendSMSContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
    }
}
