using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Entity;
using YemekTarifleri.Models;
using System.IO;

namespace YemekTarifleri.Data.Concrete.EfCore;

public class EfFoodRepository : IFoodRepository
{
    private YemekTarifleriContext _context;
    public EfFoodRepository(YemekTarifleriContext context)
    {
        _context = context;
    }

    public IQueryable<Food> Foods => _context.Foods;

    public void CreateFood(Food food)
    {
        _context.Foods.Add(food);
        _context.SaveChanges();

    }

    public void DeleteFood(Food food)
    {
        _context.Foods.Remove(food);
        _context.SaveChanges();

    }

    public void EditFood(FoodEditModels food)
    {
        var EntityFood = _context.Foods.Include(i => i.Images).Include(i => i.Ingredients).Include(s => s.Steps).Include(f => f.Ftypes).Where(c => c.Confirmation == FoodStatus.Confirm).FirstOrDefault(f => f.url == food.foodUrl);

        if (EntityFood != null)
        {
            EntityFood.isim = food.FoodName;
            EntityFood.aciklama = food.Aciklama;
            EntityFood.Confirmation = food.Confirmation;
            EntityFood.EditTime = food.EditTime;

            food.deletedImg.ForEach(di =>
            {
                EntityFood.Images.Remove(EntityFood.Images.FirstOrDefault(n => n.name == di));
                if (!EntityFood.Images.Any(i => i.type == "main"))
                {
                    EntityFood.Images[0].type = "main";
                }
            });

            food.img.ForEach(i =>
            {
                bool typeCheck = EntityFood.Images.Where(t => t.type == "main") == null ? true : false;
                string newName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(i.FileName);
                var path = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/food-img", newName);
                var stream = new FileStream(path, FileMode.Create);
                i.CopyTo(stream);


                Image image = new Image();
                image.name = newName;
                image.type = typeCheck == true ? "main" : "normal";
                image.FoodId = EntityFood.FoodId;
                EntityFood.Images.Add(image);
            });


            

            int a = 0;
            food.ingredientIDs!.ForEach(iids =>
            {
                EntityFood.Ingredients.FirstOrDefault(i => i.IngredientsID == int.Parse(iids)).Text = food.newIng[a];
                a++;
            });

            food.deletedIngredients.ForEach(di => EntityFood.Ingredients.Remove(EntityFood.Ingredients.FirstOrDefault(i => i.IngredientsID == int.Parse(di))));

            int s = 0;
            food.stepIDs!.ForEach(sids =>
            {
                EntityFood.Steps.FirstOrDefault(s => s.StepID == int.Parse(sids)).Text = food.newStep[s];
                s++;
            });

            food.deletedStep.ForEach(ds => EntityFood.Steps.Remove(EntityFood.Steps.FirstOrDefault(s => s.StepID == int.Parse(ds))!));

            EntityFood.Ftypes.Clear();
            food.ftypes.ForEach(ft => EntityFood.Ftypes.Add(_context.Ftypes.FirstOrDefault(f => f.TypeID == int.Parse(ft))));

            food.addIng.ForEach(i => EntityFood.Ingredients.Add(new Ingredient { Text = i.ToString() }));
            food.addStep.ForEach(s => EntityFood.Steps.Add(new Step { Text = s.ToString() }));

            _context.SaveChanges();
        }


    }

    public async Task ConfirmFood(Food food)
    {
        food.Confirmation = FoodStatus.Confirm;
        await _context.SaveChangesAsync();
    }

    public async Task DenialFood(Food food)
    {
        food.Confirmation = FoodStatus.Denial;
        await _context.SaveChangesAsync();
    }

    public async Task PendingFood(Food food)
    {
        food.Confirmation = FoodStatus.Pending;
        await _context.SaveChangesAsync();
    }

    public void UpdateFtypes(FoodDetails model)
    {
        if (model.ftypeIds.Count > 0)
        {
            model.food.Ftypes.Clear();
            model.ftypeIds.ForEach(ft => model.food.Ftypes.Add(_context.Ftypes.FirstOrDefault(f => f.TypeID == ft)));
            _context.SaveChanges();
        }

    }
}
