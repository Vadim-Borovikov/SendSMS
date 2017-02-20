using System.Collections.Generic;
using ServiceStack;
using SendSMS.ServiceModel;
using SendSMS.ServiceModel.Types;

namespace SendSMS.ServiceInterface
{
    public class SendSmsService : IService
    {
        public IEnumerable<Country> Get(GetCountries request)
        {
            return DataManager.GetCountries();
        }

        public Data.Types.State Get(ServiceModel.SendSMS request)
        {
            return DataManager.SendSMS(request.From, request.To, request.Text);
        }

        public GetSentSMSResponse Get(GetSentSMS request)
        {
            return DataManager.GetSentSMS(request.DateTimeFrom, request.DateTimeTo, request.Skip, request.Take);
        }
    }
}