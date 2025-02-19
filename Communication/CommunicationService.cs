using Newtonsoft.Json;
using NurseryMart.Contract;
using NurseryMart.ExternalServices;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace NurseryMart.Communication
{
    public class CommunicationService : ICommunicationService
    {
        private readonly IWhatsappProvider _whisappProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CommunicationService> _logger;
        public CommunicationService(IWhatsappProvider whisappProvider, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _whisappProvider = whisappProvider;
            _logger = loggerFactory.CreateLogger<CommunicationService>();
            _configuration = configuration;
        }

        public async Task SendTwilioWhatsAppMessage(WhatsappProviderDto whatsappProviderDto)
        {
            var mobNo = _configuration["ThirdPartyServices:Whatsapp:mobile_no"];
            var messageOptions = new CreateMessageOptions(
             new PhoneNumber($"whatsapp:+91{whatsappProviderDto.MobNo}"));
            var serviceId = _configuration["ThirdPartyServices:Whatsapp:whatsapp_service_Id"];
            messageOptions.From = new PhoneNumber($"whatsapp:{mobNo}");
            messageOptions.ContentSid = "HX4f2d067129e09f321f609f5f2766a425";
            messageOptions.MediaUrl = new List<Uri> {
            new Uri("https://dev-sportzfirst-resources-via-api.s3.ap-south-1.amazonaws.com/media/6684e1fb364a747bf814603d/822d3922-ef2c-4c4f-8615-c5d4531acac8.jpg")};

            //var message = await MessageResource.CreateAsync(
            //to: new Twilio.Types.PhoneNumber("+15558675310"),
            //messagingServiceSid: "MG9752274e9e519418a7406176694466fa",
            //body: "Do you know what time it is? It must be party time!");
            //messageOptions.MessagingServiceSid = "MG233483e899e7d72dbe96a9905fd555a9";
            string jsonString = JsonConvert.SerializeObject(whatsappProviderDto.dynamicObject);
            messageOptions.ContentVariables = jsonString;
            await _whisappProvider.TwilioSendText(messageOptions);
        }
    }
}
