using Twilio.Rest.Api.V2010.Account;

namespace NurseryMart.ExternalServices
{
    public interface IWhatsappProvider
    {
        Task TwilioSendText(CreateMessageOptions messageOptions);
    }
}
