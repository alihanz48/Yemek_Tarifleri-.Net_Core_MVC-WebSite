using System.IO.Compression;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Data.Concrete.EfCore;
using YemekTarifleri.Entity;
using YemekTarifleri.Models;

namespace YemekTarifleri.Controllers
{
    public class HomeController : Controller
    {
        private IFoodRepository _foodRepository;
        private ICommentRepository _commentRepository;
        private IUserRepository _userRepository;
        private IFtypeRepository _ftypeRepository;
        private ILikeRepository _likeRepository;
        private IViewRepository _viewRepository;

        public HomeController(IFoodRepository foodRepository, ICommentRepository commentRepository, IUserRepository userRepository, IFtypeRepository ftypeRepository, ILikeRepository likeRepository, IViewRepository viewRepository)
        {
            _foodRepository = foodRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _ftypeRepository = ftypeRepository;
            _likeRepository = likeRepository;
            _viewRepository = viewRepository;
        }

        public async Task<IActionResult> Index()
        {
            var foods = _foodRepository.Foods.Include(i => i.Images.Where(t => t.type == "main")).Include(v => v.views).Include(l => l.Likes).Include(c => c.Comments).Where(c=>c.Confirmation==FoodStatus.Confirm);
            return View(new FoodsViewModel
            {
                foods = await foods.ToListAsync()
            });

        }

        public IActionResult NotFoundPage()
        {
            return View();
        }

        public async Task<IActionResult> Yemek_turu(string tur)
        {
            var foods = _foodRepository.Foods.Where(t => t.Ftypes.Any(t => t.Url == tur)).Include(l => l.Likes).Include(c => c.Comments).Include(v => v.views).Include(i => i.Images.Where(i => i.type == "main")).Where(c=>c.Confirmation==FoodStatus.Confirm).ToList();
            ViewData["tur"] = _ftypeRepository.Ftypes.FirstOrDefault(ft => ft.Url == tur)?.Name;
            return View(new FoodOfTypesViewModel
            {
                FoodsOfType = foods
            });
        }



        public IActionResult Details(string url)
        {

            var food = _foodRepository.Foods.Include(u => u.Users).Include(i => i.Ingredients).Include(s => s.Steps).Include(c => c.Comments).ThenInclude(cu => cu.Users).Include(i => i.Images).Include(ft => ft.Ftypes).Include(i => i.Likes).Include(v => v.views).FirstOrDefault(u => u.url == url);

            if (!food.views.Any(ed => ed.EnterDetail == User.FindFirstValue(ClaimTypes.Actor) || ed.EnterDetail == System.Environment.MachineName))
            {
                View view = new View();
                if (User.Identity!.IsAuthenticated)
                {
                    view.EnterDetail = User.FindFirstValue(ClaimTypes.Actor);
                }
                else
                {
                    view.EnterDetail = System.Environment.MachineName;
                }
                view.time = DateTime.Now;
                view.FoodId = food.FoodId;
                _viewRepository.CreateView(view);
            }

            if (food == null||food.Confirmation!=FoodStatus.Confirm)
            {
                return View("NoFood");
            }

            ViewData["Food_name"] = food.isim;
            return View(new FoodDetails
            {
                food = food,
                ftypes=_ftypeRepository.Ftypes.ToList()
            });

        }

        [HttpPost]
        public JsonResult LikeCheck(string foodId, string userId)
        {
            int _foodId = int.Parse(foodId);
            int _userId = int.Parse(userId);

            if (_likeRepository.Likes.Any(l => l.foodId == _foodId && l.userId == _userId))
            {
                _likeRepository.DeleteLike(_likeRepository.Likes.FirstOrDefault(l => l.foodId == _foodId && l.userId == _userId));
            }
            else
            {
                _likeRepository.CreateLike(new Like
                {
                    foodId = _foodId,
                    userId = _userId
                });
            }

            String likeit = _likeRepository.Likes.Any(l => l.foodId == _foodId && l.userId == _userId).ToString();
            String likeCount = _foodRepository.Foods.Include(l => l.Likes).FirstOrDefault(f => f.FoodId == _foodId).Likes.Count().ToString();

            return Json(new
            {
                likeit,
                likeCount
            });
        }

        [HttpPost]
        public JsonResult AddComment(String FoodID, String text)
        {
            var entityComment = new Comment
            {
                Text = text,
                PublishDate = DateTime.Now,
                UserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                FoodID = int.Parse(FoodID)
            };

            String avatarName = User.FindFirstValue(ClaimTypes.UserData)!;
            String userName = User.FindFirstValue(ClaimTypes.Email)!;
            String commentDate = entityComment.PublishDate.ToShortDateString();

            int commentId = _commentRepository.CreateComment(entityComment);

            return Json(new
            {
                commentId,
                avatarName,
                userName,
                commentDate,
                text
            });
        }

        [HttpPost]
        public JsonResult DelComment(string CommentId)
        {
            _commentRepository.DeleteComment(_commentRepository.Comments.FirstOrDefault(c=>c.CommentID==int.Parse(CommentId)));
            bool success = true;
            return Json(new
            {
             success
            });
        }  

        [HttpPost]
        public async Task<IActionResult> Search(SearchDataViewModel model)
        {

            var foods = _foodRepository.Foods.Where(f => f.isim.Contains(model.query));
            if (model.category != "-1")
            {
                foods = foods.Where(f => f.Ftypes.Any(c => c.Url == model.category));
            }
            foods = foods.Include(i => i.Images.Where(i => i.type == "main")).Include(l=>l.Likes).Include(c=>c.Comments).Include(v=>v.views);

            return View(new SearchDataViewModel
            {
                foods = await foods.ToListAsync(),
            });
        }

    }
}