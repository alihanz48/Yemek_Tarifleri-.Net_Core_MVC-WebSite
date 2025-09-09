using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Data.Concrete.EfCore;
using YemekTarifleri.Entity;
using YemekTarifleri.Models;
using System.IO;
using System;
using SecureTokenGeneratR;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace YemekTarifleri.Controllers
{
    public class FoodController : Controller
    {
        private IFtypeRepository _ftypeRepository;
        private IFoodRepository _foodRepository;
        private IAuthorizationService _authorizationService;
        public FoodController(IFtypeRepository ftypeRepository, IFoodRepository foodRepository, IAuthorizationService authorizationService)
        {
            _ftypeRepository = ftypeRepository;
            _foodRepository = foodRepository;
            _authorizationService = authorizationService;
        }

        [Authorize(Policy = "isLogin")]
        public async Task<IActionResult> AddFood()
        {
            AddFoodViewModel food = new AddFoodViewModel();
            food.Ftypes = _ftypeRepository.Ftypes.ToList();
            return View(food);
        }

        [HttpPost]
        public JsonResult AddFood(string isim, string aciklama, string userID, String[] ingredients, String[] steps, int[] ftypeIDs, List<IFormFile> img)
        {
            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredients.ToList().ForEach(item=>ingredientList.Add(new Ingredient{Text=item}));

            List<Step> stepList = new List<Step>();
            steps.ToList().ForEach(item=>stepList.Add(new Step{Text=item}));

            List<Image> imageList = new List<Image>();
            int i = 0;
            img.ToList().ForEach(item=>
            {
                string newName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(item.FileName);
                imageList.Add(new Image { name = newName, type = i == 0 ? "main" : "normal" });
                var path = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/food-img", newName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    item.CopyTo(stream);
                    stream.Dispose();
                    stream.Close();
                }
            });


            var securetokengenerator = new SecureTokenGenerator();

            _foodRepository.CreateFood(new Food
            {
                isim = isim,
                aciklama = aciklama,
                tarih = DateTime.Now,
                url = securetokengenerator.Generate().Replace("/", "a").Replace("\\", "b").Replace("?", "c").Replace("#", "d"),
                UserID = int.Parse(userID),
                Confirmation = FoodStatus.Pending,
                Ingredients = ingredientList,
                Steps = stepList,
                Images = imageList,
                Ftypes = _ftypeRepository.Ftypes.Where(t => ftypeIDs.Contains(t.TypeID)).ToList()
            });
            return Json(new { success = "success" });
        }

        [Authorize(Policy = "isLogin")]
        [Authorize(Policy = "isOwner")]
        public async Task<IActionResult> RemoveFood(string url)
        {

            var food = _foodRepository.Foods.Where(c => c.Confirmation == FoodStatus.Confirm||c.Confirmation==FoodStatus.Denial).FirstOrDefault(f => f.url == url);
            if (food != null)
            {
                return View(food);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public IActionResult RemoveFood(Food model)
        {

            var foodAndPhotos = _foodRepository.Foods.Include(i => i.Images).Where(c => c.Confirmation == FoodStatus.Confirm||c.Confirmation==FoodStatus.Denial).FirstOrDefault(f => f.FoodId == model.FoodId);

            foodAndPhotos.Images.ForEach(i => System.IO.File.Delete(Path.Join(Directory.GetCurrentDirectory(), "wwwroot/food-img", i.name)));

            _foodRepository.DeleteFood(foodAndPhotos);


            return Redirect("/Users/MyAccount");
        }

        [Authorize(Policy = "isLogin")]
        [Authorize(Policy = "isOwner")]
        public async Task<IActionResult> FoodEdit(string url)
        {
            var food = _foodRepository.Foods.Include(i => i.Images).Include(i => i.Ingredients).Include(s => s.Steps).Include(f => f.Ftypes).Where(c => c.Confirmation == FoodStatus.Confirm).FirstOrDefault(f => f.url == url);

            if (food != null)
            {
                List<string> foodIng = new List<string>();
                List<string> steps = new List<string>();
                List<string> images = new List<string>();
                List<int> ftypesID = new List<int>();

                food.Ingredients.ToList().ForEach(x => foodIng.Add(x.Text));
                food.Steps.ToList().ForEach(x => steps.Add(x.Text));

                food.Images.ToList().ForEach(x => images.Add(x.name));
                food.Ftypes.ToList().ForEach(x => ftypesID.Add(Convert.ToInt32(x.TypeID)));

                return View(new AddFoodViewModel
                {
                    isim = food.isim,
                    aciklama = food.aciklama,
                    ingredients = foodIng,
                    steps = steps,
                    Ftypes = _ftypeRepository.Ftypes.ToList(),
                    imgForEdit = images,
                    selectFtypes = ftypesID,
                    foodUrl = url
                });
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public JsonResult FoodEdit(string FoodName, string Aciklama, List<string> deletedImg, List<IFormFile> img, List<string> deletedIngredients, List<string> oldIng, List<string> newIng, List<string> deletedStep, List<string> oldstep, List<string> newstep, string url, List<string> ftypes, List<string> addStep, List<string> addIng)
        {

            _foodRepository.EditFood(new FoodEditModels
            {
                foodUrl = url,
                FoodName = FoodName,
                Aciklama = Aciklama,
                Confirmation = FoodStatus.Pending,
                EditTime = DateTime.Now.Date,
                deletedImg = deletedImg,
                img = img,
                deletedIngredients = deletedIngredients,
                oldIng = oldIng,
                newIng = newIng,
                deletedStep = deletedStep,
                oldstep = oldstep,
                newstep = newstep,
                ftypes = ftypes,
                addIng = addIng,
                addStep = addStep
            });

            return Json(new { Success = "aa"});
        }
    }
}