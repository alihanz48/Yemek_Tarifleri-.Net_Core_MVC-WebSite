using YemekTarifleri.Entity;

namespace YemekTarifleri.Models
{
    public class CreateFtypes
    {
        public List<Ftype> Ftypes { get; set; } = new();

        public string FtypeName { get; set; }
    }
}