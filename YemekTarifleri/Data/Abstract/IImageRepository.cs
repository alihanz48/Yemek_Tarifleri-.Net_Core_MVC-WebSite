using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Abstract
{
    public interface IImageRepository
    {
        IQueryable<Image> Images { get; }

        void CreateImage(Image image);
        void EditImage(Image image);
        void DeleteImage(Image image);
    }
}