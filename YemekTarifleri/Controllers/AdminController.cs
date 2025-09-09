using System.IO.Compression;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Entity;
using YemekTarifleri.Models;

namespace YemekTarifleri.Controllers
{
    public class AdminController : Controller
    {
        private IFoodRepository _foodRepository;
        private IFtypeRepository _ftypeRepository;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        public AdminController(IFoodRepository foodRepository, IFtypeRepository ftypeRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _foodRepository = foodRepository;
            _ftypeRepository = ftypeRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        [Authorize(Policy = "isAdmin")]
        public IActionResult WaitConfirmation()
        {
            return View(new FoodsViewModel
            {
                foods = _foodRepository.Foods.Include(i => i.Images.Where(t => t.type == "main")).Where(f => f.Confirmation == FoodStatus.Pending).ToList(),
            });
        }

        [Authorize(Policy = "isAdmin")]
        public IActionResult view(string id)
        {
            var food = _foodRepository.Foods.Include(u => u.Users).Include(i => i.Ingredients).Include(s => s.Steps).Include(c => c.Comments).ThenInclude(cu => cu.Users).Include(i => i.Images).Include(ft => ft.Ftypes).Include(i => i.Likes).Include(v => v.views).FirstOrDefault(u => u.url == id);
            ViewData["Food_name"] = food.isim;
            return View(new FoodDetails
            {
                food = food,
                ftypes=_ftypeRepository.Ftypes.ToList()
            });
        }

        [Authorize(Policy = "isAdmin")]
        public async Task<IActionResult> FoodConfirm(string id)
        {
            await _foodRepository.ConfirmFood(_foodRepository.Foods.FirstOrDefault(f => f.url == id)!);
            return RedirectToAction("WaitConfirmation");
        }

        [Authorize(Policy = "isAdmin")]
        public async Task<IActionResult> FoodDenial(string id)
        {
            await _foodRepository.DenialFood(_foodRepository.Foods.FirstOrDefault(f => f.url == id));
            return RedirectToAction("WaitConfirmation");
        }

        [Authorize(Policy = "isAdmin")]
        public IActionResult CreateFtypes()
        {
            return View(new CreateFtypes
            {
                Ftypes = _ftypeRepository.Ftypes.ToList()
            });
        }

        [Authorize(Policy = "isAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateFtypes(CreateFtypes model)
        {
            _ftypeRepository.CreateFtypes(new Ftype
            {
                Name = model.FtypeName,
                Url = model.FtypeName.ToLower(),
            });

            return RedirectToAction("CreateFtypes");
        }

        [Authorize(Policy = "isAdmin")]
        public IActionResult RemoveFtypes(int id)
        {
            _ftypeRepository.DeleteFtypes(_ftypeRepository.Ftypes.FirstOrDefault(ft => ft.TypeID == id));
            return RedirectToAction("CreateFtypes");
        }

        [Authorize(Policy = "isAdmin")]
        [HttpPost]
        public JsonResult EditFtypes(string text, string id)
        {
            _ftypeRepository.EditFtypes(new Ftype
            {
                Name = text,
                TypeID = int.Parse(id),
            });
            return Json(new { Sonuc = "dogru" });
        }

        [Authorize(Policy = "isAdmin")]
        public IActionResult UserManagement()
        {
            var users = _userRepository.Users.Include(r => r.Roles).ToList();
            return View(new setRolesModel
            {
                users = users,
                roles = _roleRepository.Roles.ToList()
            });
        }

        [Authorize(Policy = "isAdmin")]
        public IActionResult setRole(setRolesModel model)
        {
            if (model.RoleId != -1)
            {
                var user = _userRepository.Users.FirstOrDefault(u => u.UserId == model.UserId);
                user.RoleId = model.RoleId;
                _userRepository.EditUser(user);
            }
            return RedirectToAction("UserManagement");
        }

        [Authorize(Policy = "isAdmin")]
        public async Task<IActionResult> RemoveConfirmation(string id)
        {
            Console.WriteLine("--------------------------------------------------------" + id);
            await _foodRepository.PendingFood(_foodRepository.Foods.FirstOrDefault(f => f.url == id));
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Policy = "isAdmin")]
        [HttpPost]
        public async Task<IActionResult> setFtype(FoodDetails model,string returnUrl)
        {
            model.food = _foodRepository.Foods.Include(ft => ft.Ftypes).FirstOrDefault(f => f.FoodId == model.FoodId);
            _foodRepository.UpdateFtypes(model);
            return Redirect(returnUrl);
        }

    }
}