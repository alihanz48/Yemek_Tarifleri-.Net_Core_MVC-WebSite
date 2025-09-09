using System.ComponentModel.DataAnnotations;

namespace YemekTarifleri.Entity;

public enum FoodStatus
{
    Pending = 0,
    Confirm = 1,
    Denial=2,
}

public class Food
{
    [Key]
    public int FoodId { get; set; }
    public string isim { get; set; }
    public string aciklama { get; set; }
    public DateTime tarih { get; set; }
    public string url { get; set; }

    public List<View> views { get; set; } = new List<View>();

    public int UserID { get; set; }
    public User Users { get; set; } = null!;

    public FoodStatus Confirmation { get; set; }

    public DateTime? EditTime { get; set; }

    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public List<Step> Steps { get; set; } = new List<Step>();

    public List<Comment> Comments { get; set; } = new List<Comment>();

    public List<Image> Images { get; set; } = new List<Image>();

    public List<Ftype> Ftypes { get; set; } = new List<Ftype>();

    public List<Like> Likes { get; set; } = new List<Like>();
}