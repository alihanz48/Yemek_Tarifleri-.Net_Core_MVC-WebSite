using System.ComponentModel.DataAnnotations;

namespace YemekTarifleri.Entity;

public class Ftype
{
    [Key]
    public int TypeID { get; set; }

    public string Url { get; set; }

    public string Name { get; set; }

    public List<Food> Foods { get; set; } = new List<Food>();
}