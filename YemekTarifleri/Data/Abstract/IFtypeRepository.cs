using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Abstract
{
    public interface IFtypeRepository
    {
        IQueryable<Ftype> Ftypes { get; }

        void CreateFtypes(Ftype ftype);
        void EditFtypes(Ftype ftype);
        void DeleteFtypes(Ftype ftype);
    }
}