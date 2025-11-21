using CORE.APP.Domain;

namespace APP.Domain
{
    // Join tablosu: User–Role many-to-many ilişkisinin ortadaki entity’si
    public class UserRole : Entity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
