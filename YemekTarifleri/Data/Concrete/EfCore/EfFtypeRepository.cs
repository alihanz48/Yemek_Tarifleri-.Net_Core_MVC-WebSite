using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Concrete.EfCore;

public class EfFtypeRepository : IFtypeRepository
{
    private YemekTarifleriContext _context;
    public EfFtypeRepository(YemekTarifleriContext context)
    {
        _context = context;
    }

    public IQueryable<Ftype> Ftypes => _context.Ftypes;

    public void CreateFtypes(Ftype ftype)
    {
        _context.Ftypes.Add(ftype);
        _context.SaveChanges();
    }

    public void DeleteFtypes(Ftype ftype)
    {
        _context.Ftypes.Remove(ftype);
        _context.SaveChanges();
    }

    public void EditFtypes(Ftype ftype)
    {
        var FtypesEntity = _context.Ftypes.FirstOrDefault(ft => ft.TypeID == ftype.TypeID);

        if (FtypesEntity != null)
        {
            FtypesEntity.Name = ftype.Name;
            FtypesEntity.Url = ftype.Name.ToLower().Replace(" ","-");

            _context.SaveChanges();
        }
    }
}
