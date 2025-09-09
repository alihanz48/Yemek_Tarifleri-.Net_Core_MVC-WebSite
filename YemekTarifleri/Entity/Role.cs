using System.ComponentModel.DataAnnotations;
using YemekTarifleri.Entity;

public class Role
{
    [Key]
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public List<User> Users { get; set; } = new List<User>();
}