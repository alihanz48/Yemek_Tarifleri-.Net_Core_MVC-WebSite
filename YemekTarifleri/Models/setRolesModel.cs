using YemekTarifleri.Entity;

namespace YemekTarifleri.Models
{
    public class setRolesModel
    {
        public List<User> users { get; set; } = new();
        public List<Role> roles { get; set; } = new();


        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
    
}