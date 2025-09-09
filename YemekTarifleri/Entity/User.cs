using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Runtime.Serialization.Formatters.Binary;

namespace YemekTarifleri.Entity;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string? avatarName { get; set; }

    public string Name { get; set; }

    public string username { get; set; }

    public string mail { get; set; }

    public byte[] password { get; set; }

    public DateTime KayitTarihi { get; set; }

    public int RoleId { get; set; }
    public Role Roles { get; set; } = null!;
    
    
    public List<Food> Foods { get; set; } = new List<Food>();
    public List<Comment> Comments { get; set; } = new List<Comment>();

    public List<Like> Likes { get; set; } = new List<Like>();

}