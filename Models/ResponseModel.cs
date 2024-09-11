namespace Api_Template.Models
{
    public class ResponseModel
    {
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
