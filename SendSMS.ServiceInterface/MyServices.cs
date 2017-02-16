using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;
using SendSMS.ServiceModel;

namespace SendSMS.ServiceInterface
{
    public class MyServices : IService
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = "Hello, {0}!".Fmt(request.Name) };
        }
    }
}