using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Abstract
{
    public interface ILikeRepository
    {
        IQueryable<Like> Likes { get; }

        void CreateLike(Like like);
        void DeleteLike(Like like);
    }
}