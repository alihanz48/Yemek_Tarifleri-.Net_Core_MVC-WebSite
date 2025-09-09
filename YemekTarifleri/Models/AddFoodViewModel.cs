using System.ComponentModel.DataAnnotations;
using YemekTarifleri.Entity;

namespace YemekTarifleri.Models{
    public class AddFoodViewModel
    {
        public string? isim { get; set; }
        public string? aciklama { get; set; }

        public List<string>? ingredients { get; set; }

        public List<string>? steps { get; set; }

        public List<Ftype>? Ftypes { get; set; }
        public List<IFormFile>? Files { get; set; }

        public List<string>? imgForEdit { get; set; }
        public List<int>? selectFtypes { get; set; }

        public string? foodUrl { get; set; }
    }
}