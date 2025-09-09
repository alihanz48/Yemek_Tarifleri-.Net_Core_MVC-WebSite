using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Abstract
{
    public interface IUserRepository
    {

        IQueryable<User> Users { get; }

        void CreateUser(User user);

        void DeleteUser(User user);

        void EditUser(User user);
    }
}