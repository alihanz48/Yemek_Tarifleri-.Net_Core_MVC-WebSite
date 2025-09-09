using System.ComponentModel.DataAnnotations;

namespace YemekTarifleri.Entity;

public class Like
{
    [Key]
    public int LikeId { get; set; }
    public int foodId { get; set; }
    public Food? foods { get; set; } = null!;
    public int userId { get; set; }
    public User? users { get; set; } = null!;
}