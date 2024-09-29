namespace API_TEMPLATE.Data.Users
{
    public class RoleMaster
    {
        public RoleMaster()
        {
            GUID = Guid.NewGuid().ToString();
        }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string GUID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<UserMaster> Users { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
