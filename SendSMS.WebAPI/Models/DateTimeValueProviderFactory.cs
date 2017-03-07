using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace SendSMS.WebAPI.Models
{
    internal class DateTimeValueProviderFactory : ValueProviderFactory, IUriValueProviderFactory
    {
        private readonly string _format;

        public DateTimeValueProviderFactory(string format)
        {
            _format = format;
        }

        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            return new DateTimeValueProvider(actionContext, _format);
        }
    }
}