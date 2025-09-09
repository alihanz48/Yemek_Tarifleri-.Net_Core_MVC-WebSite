using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace YemekTarifleri.Entity;

public class Image
{
    [Key]
    public int ImageID { get; set; }

    public string name { get; set; }
    public string type { get; set; }

    public int FoodId { get; set; }
    public Food food { get; set; } = null!;
}