using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace YemekTarifleri.Entity;

public class Step
{
    [Key]
    public int StepID { get; set; }

    public string Text { get; set; }

    public int FoodID { get; set; }
    public Food Foods { get; set; } = null!;
}