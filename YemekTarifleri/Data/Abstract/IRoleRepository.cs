using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Abstract
{
    public interface IRoleRepository{
        IQueryable<Role> Roles { get; }
    }
}