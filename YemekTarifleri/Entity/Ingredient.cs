using System.ComponentModel.DataAnnotations;

namespace YemekTarifleri.Entity;

public class Ingredient
{
    [Key]
    public int IngredientsID { get; set; }

    public string Text { get; set; }

    public int FoodID { get; set; }
    public Food Foods { get; set; } = null!;
    
}