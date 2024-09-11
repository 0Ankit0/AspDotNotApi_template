namespace Api_Template.Models
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenLifetimeHours { get; set; } = 10;
    }

    public class AuthenticatedUser
{
    public string UserId { get; set; }
    public string Username { get; set; }
}
}