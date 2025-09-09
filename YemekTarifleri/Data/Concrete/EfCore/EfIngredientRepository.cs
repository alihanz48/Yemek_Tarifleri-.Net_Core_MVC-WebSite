using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Concrete.EfCore;

public class EfIngredientRepository : IIngredientRepository
{
    private YemekTarifleriContext _context;
    public EfIngredientRepository(YemekTarifleriContext context)
    {
        _context = context;
    }

    public IQueryable<Ingredient> Ingredients => _context.Ingredients;

    public void CreatIngredient(Ingredient ingredient)
    {
        _context.Ingredients.Add(ingredient);
        _context.SaveChanges();   
    }

    public void DeleteIngredient(Ingredient ingredient)
    {
        _context.Ingredients.Remove(ingredient);
        _context.SaveChanges();   
    }

    public void EditIngredient(Ingredient ingredient)
    {
        var IngredientEntity = _context.Ingredients.Where(i => i.IngredientsID == ingredient.IngredientsID);

        if (IngredientEntity != null)
        {




            _context.SaveChanges();
        }
    }
}
