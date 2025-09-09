using YemekTarifleri.Data.Abstract;

namespace YemekTarifleri.Data.Concrete.EfCore;

public class EfRoleRepository : IRoleRepository
{
    private YemekTarifleriContext _context;
    public EfRoleRepository(YemekTarifleriContext context)
    {
        _context = context;
    }
    public IQueryable<Role> Roles => _context.Roles;
}
