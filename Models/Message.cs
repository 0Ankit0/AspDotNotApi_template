namespace ServiceApp_backend.Models
{
    public class Message
    {
        public string MessageText { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public string TokenNo { get; set; }
    }
}
