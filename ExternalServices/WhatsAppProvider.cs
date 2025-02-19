using NurseryMart.Contract;
using System.Net;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace NurseryMart.ExternalServices
{
    public class WhatsAppProvider : IWhatsappProvider
    {
        private readonly IConfiguration _configuration;
        //private readonly ICacheProvider _cacheProvider;
        //private readonly ILogger<SmsProvider> _logger;
        public WhatsAppProvider( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task TwilioSendText(CreateMessageOptions messageOptions)
        {
            var accountSid = _configuration["ThirdPartyServices:Whatsapp:account_sid"];
            var authToken = _configuration["ThirdPartyServices:Whatsapp:auth_oken"];
            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(messageOptions);
            if (message.Status == MessageResource.StatusEnum.Failed) throw new RestException(HttpStatusCode.BadRequest, "Failed send Whatsapp, please try after sometime and if error still persist please contact support.");
        }

    }
}
