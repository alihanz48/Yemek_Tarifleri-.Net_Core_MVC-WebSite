using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Concrete.EfCore;

public class EfViewRepository : IViewRepository
{
    private YemekTarifleriContext _context;
    public EfViewRepository(YemekTarifleriContext context)
    {
        _context = context;
    }

    public IQueryable<View> Views => _context.Views;

    public void CreateView(View view)
    {
        _context.Add(view);
        _context.SaveChanges();
    }
}