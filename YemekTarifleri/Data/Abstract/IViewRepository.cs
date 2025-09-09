using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Abstract
{
    public interface IViewRepository
    {
        IQueryable<View> Views { get; }

         void CreateView(View view);
    }
}