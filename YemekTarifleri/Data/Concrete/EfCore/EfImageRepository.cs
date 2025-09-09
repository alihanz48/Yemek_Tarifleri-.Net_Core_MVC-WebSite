using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Concrete.EfCore;

public class EfImageRepository : IImageRepository
{
    private YemekTarifleriContext _context;
    public EfImageRepository(YemekTarifleriContext context)
    {
        _context = context;
    }

    public IQueryable<Image> Images => _context.Images;

    public void CreateImage(Image image)
    {
        _context.Images.Add(image);
        _context.SaveChanges();
    }
    public void DeleteImage(Image image)
    {
        _context.Images.Remove(image);
        _context.SaveChanges();
    }

    public void EditImage(Image image)
    {
        var ImageEntity = _context.Images.Where(i => i.ImageID == image.ImageID);
        if (ImageEntity != null)
        {





            _context.SaveChanges();
        }
    }
}
