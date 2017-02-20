using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SendSMS.Data.Types;
using ServiceStack;
using ServiceStack.Testing;
using SendSMS.ServiceInterface;
using SendSMS.ServiceModel;
using ServiceStack.Validation;
using Country = SendSMS.ServiceModel.Types.Country;

namespace SendSMS.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private readonly ServiceStackHost _appHost;

        public UnitTests()
        {
            _appHost = new BasicAppHost(typeof(SendSmsService).Assembly)
            {
                ConfigureContainer = container => container.RegisterValidators(typeof(SendSMSValidator).Assembly)
            }
            .Init();
        }

        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
            _appHost.Dispose();
        }

        [Test]
        public void Test_GetCountries()
        {
            var service = _appHost.Container.Resolve<SendSmsService>();

            IEnumerable<Country> response = service.Get(new GetCountries());

            Assert.That(response.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Test_SendSMS()
        {
            var service = _appHost.Container.Resolve<SendSmsService>();

            State response = service.Get(new ServiceModel.SendSMS { To = "+4917421293388", Text = "Hello" });
            Assert.That(response, Is.EqualTo(State.Success));

            response = service.Get(new ServiceModel.SendSMS { From = "Goofy", To = "+8800807775533", Text = "Gawrsh!" });
            Assert.That(response, Is.EqualTo(State.Failed));
        }

        [Test]
        public void Test_GetSentSMS()
        {
            var service = _appHost.Container.Resolve<SendSmsService>();

            GetSentSMSResponse response = service.Get(new GetSentSMS());

            Assert.That(response.TotalCount, Is.GreaterThan(0));
        }
    }
}
