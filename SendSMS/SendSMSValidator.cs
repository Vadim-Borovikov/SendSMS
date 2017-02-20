using ServiceStack.FluentValidation;

namespace SendSMS
{
    public class SendSMSValidator : AbstractValidator<ServiceModel.SendSMS>
    {
        public SendSMSValidator()
        {
            RuleFor(x => x.From).NotEmpty();
        }
    }
}