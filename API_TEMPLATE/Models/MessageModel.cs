namespace API_TEMPLATE.Models
{
    public class MessageModel
    {
        public string MessageText { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string TokenNo { get; set; }
    }
    public class MapToHubId
    {
        public string Publickey { get; set; }
        public string Privatekey { get; set; }
        public string UserId { get; set; }
    }
}
