using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Abstract
{
    public interface IStepRepository
    {
        IQueryable<Step> Steps { get; }

        void CreateStep(Step step);
        void DeleteStep(Step step);
        void EditStep(Step step);
    }
}