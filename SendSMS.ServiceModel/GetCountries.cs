using System.Collections.Generic;
using SendSMS.ServiceModel.Types;
using ServiceStack;

namespace SendSMS.ServiceModel
{
    [Route("/countries", "GET")]
    public class GetCountries : IReturn<IEnumerable<Country>>
    {
    }
}