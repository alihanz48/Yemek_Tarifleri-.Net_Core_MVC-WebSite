using YemekTarifleri.Entity;

namespace YemekTarifleri.Models
{
    public class FoodDetails
    {
        public Food food { get; set; }
        public List<Ftype>? ftypes { get; set; } = new();
        public List<int>? ftypeIds { get; set; } = new();
        public int FoodId { get; set; }
    }
}