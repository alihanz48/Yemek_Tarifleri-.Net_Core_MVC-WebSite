using Microsoft.EntityFrameworkCore;
using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Concrete.EfCore;

public class EfCommentRepository : ICommentRepository
{

    private YemekTarifleriContext _context;
    public EfCommentRepository(YemekTarifleriContext context)
    {
        _context = context;
    }

    public IQueryable<Comment> Comments => _context.Comments;

    public int CreateComment(Comment comment)
    {
        var cid = _context.Comments.Add(comment);
        _context.SaveChanges();
        
        return Convert.ToInt32(cid.Entity.CommentID);
    }

    public void DeleteComment(Comment comment)
    {
        _context.Comments.Remove(comment);
        _context.SaveChanges();
    }

    public void EditComment(Comment comment)
    {
        var CommentEntity = _context.Comments.Where(c => c.CommentID == comment.CommentID);

        if (CommentEntity != null)
        {





            _context.SaveChanges();
        }
    }
}
