using YemekTarifleri.Entity;

namespace YemekTarifleri.Models
{
    public class SearchDataViewModel
    {
        public List<Ftype>? ftypes { get; set; } = new();
        public List<Food>? foods { get; set; } = new();

        public string? query { get; set; }
        
        public string? category{ get; set; }
    }
}