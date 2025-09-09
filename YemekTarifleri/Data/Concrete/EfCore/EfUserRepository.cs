using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Concrete.EfCore;

public class EfUserRepository : IUserRepository
{
    private YemekTarifleriContext _context;
    public EfUserRepository(YemekTarifleriContext context)
    {
        _context = context;
    }

    public IQueryable<User> Users => _context.Users;

    public void CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void DeleteUser(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public void EditUser(User user)
    {
        var UserEntity = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);

        if (UserEntity != null)
        {
            UserEntity.Name = user.Name;
            UserEntity.username = user.username;
            UserEntity.mail = user.mail;
            UserEntity.RoleId = user.RoleId;
            _context.SaveChanges();
        }

        
    }
}
