using System.ComponentModel.DataAnnotations;

namespace YemekTarifleri.Entity;

public class Comment
{
    [Key]
    public int CommentID { get; set; }

    public string Text { get; set; }

    public DateTime PublishDate { get; set; }

    public int UserID { get; set; }
    public User? Users { get; set; } = null!;

    public int FoodID { get; set; }
    public Food? Foods { get; set; } = null!;
}