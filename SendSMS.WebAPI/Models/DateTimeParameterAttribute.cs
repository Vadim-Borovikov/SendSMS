using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace SendSMS.WebAPI.Models
{
    internal class DateTimeParameterAttribute : ModelBinderAttribute
    {
        public string Format { get; set; }

        public override IEnumerable<ValueProviderFactory> GetValueProviderFactories(HttpConfiguration configuration)
        {
            yield return new DateTimeValueProviderFactory(Format);
        }
    }
}