using YemekTarifleri.Entity;
using YemekTarifleri.Models;

namespace YemekTarifleri.Data.Abstract
{
    public interface IFoodRepository
    {
        IQueryable<Food> Foods { get; }

        void CreateFood(Food food);

        void EditFood(FoodEditModels food);

        void DeleteFood(Food food);
        Task ConfirmFood(Food food);
        Task DenialFood(Food food);
        Task PendingFood(Food food);

        void UpdateFtypes(FoodDetails model);
    }
}
