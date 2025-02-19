using NurseryMart.Contract;

namespace NurseryMart.Communication
{
    public interface ICommunicationService
    {
        Task SendTwilioWhatsAppMessage(WhatsappProviderDto whatsappProviderDto);

    }
}
