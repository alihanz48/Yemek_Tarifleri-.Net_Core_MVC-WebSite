using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Concrete.EfCore;

public class EfLikeRepository : ILikeRepository
{
    private YemekTarifleriContext _context;
    public EfLikeRepository(YemekTarifleriContext context)
    {
        _context = context;
    }

    public IQueryable<Like> Likes => _context.Likes;

    public void CreateLike(Like like)
    {

        _context.Likes.Add(like);
        _context.SaveChanges();

    }

    public void DeleteLike(Like like)
    {

        _context.Likes.Remove(like);
        _context.SaveChanges();
        
    }
}
