namespace API_TEMPLATE.Models.Users
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string GUID { get; set; }
    }
}
