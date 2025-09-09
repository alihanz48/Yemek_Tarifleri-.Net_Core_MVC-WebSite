using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Abstract
{
   public interface ICommentRepository
   {
      IQueryable<Comment> Comments { get; }

      int CreateComment(Comment comment);
      void DeleteComment(Comment comment);
      void EditComment(Comment comment);
   } 
}