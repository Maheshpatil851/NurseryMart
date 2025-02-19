namespace NurseryMart.Contract
{
    public class WhatsappProviderDto
    {
        public string ContentId { get; set; }
        public Dictionary<string, object>? dynamicObject { get; set; }
        public string MobNo { get; set; }
    }
}
