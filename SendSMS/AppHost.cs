using Funq;
using ServiceStack;
using SendSMS.ServiceInterface;
using SendSMS.ServiceModel;
using ServiceStack.Validation;

namespace SendSMS
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("SendSMS", typeof(SendSmsService).Assembly) { }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            Plugins.Add(new ValidationFeature());
            // container.RegisterValidators(typeof(SendSMSValidator).Assembly);
        }
    }
}