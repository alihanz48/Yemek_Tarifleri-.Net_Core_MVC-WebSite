using YemekTarifleri.Entity;

namespace YemekTarifleri.Models
{
    public class FoodEditModels
    {
        public string? foodUrl { get; set; }
        public string? FoodName { get; set; }
        public string? Aciklama { get; set; }
        public FoodStatus Confirmation { get; set; }
        public DateTime? EditTime { get; set; }
        public List<string> deletedImg { get; set; }
        public List<IFormFile>? img { get; set; }
        public List<string>? deletedIngredients { get; set; }
        public List<string>? oldIng { get; set; }
        public List<string>? newIng { get; set; }
        public List<string>? deletedStep { get; set; }
        public List<string>? oldstep { get; set; }
        public List<string>? newstep { get; set; }
        public List<string>? ftypes { get; set; }
        public List<string> addIng { get; set; }
        public List<string> addStep { get; set; }
    }
}