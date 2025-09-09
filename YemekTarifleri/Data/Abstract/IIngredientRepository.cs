using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Abstract
{
    public interface IIngredientRepository
    {
        IQueryable<Ingredient> Ingredients { get; }

        void CreatIngredient(Ingredient ingredient);
        void DeleteIngredient(Ingredient ingredient);
        void EditIngredient(Ingredient ingredient);
    }
}
