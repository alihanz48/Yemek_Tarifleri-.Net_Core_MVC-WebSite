using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using YemekTarifleri.Data.Concrete.EfCore;
using YemekTarifleri.Models;
using System.Security.Cryptography;
using System.Text;
using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace YemekTarifleri
{
    public class UsersController : Controller
    {

        private IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            ViewData["result"] = HttpContext.Request.Query["result"];
            ViewData["returnUrl"] = HttpContext.Request.Query["returnUrl"] == "" ? "" : HttpContext.Request.Query["returnUrl"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? fUrl)
        {
            UTF8Encoding utf8encoding = new UTF8Encoding();
            if (ModelState.IsValid)
            {
                var userCheck = _userRepository.Users.Include(r=>r.Roles).FirstOrDefault(x => (x.username == model.passString || x.mail == model.passString) && x.password == SHA256.HashData(utf8encoding.GetBytes(model.password)));
                if (userCheck != null)
                {

                    var userClaims = new List<Claim>();
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userCheck.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Email, userCheck.username));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, userCheck.Name));
                    userClaims.Add(new Claim(ClaimTypes.Role, userCheck.Roles.RoleName));
                    userClaims.Add(new Claim(ClaimTypes.UserData, userCheck.avatarName!));
                    userClaims.Add(new Claim(ClaimTypes.Actor, userCheck.mail));

                    var ClaimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authPrpperties = new AuthenticationProperties
                    {
                        IsPersistent = model.beniHatirla == "false" ? false : true,
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(ClaimsIdentity),
                        authPrpperties
                    );

                    if (fUrl == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(fUrl);
                    }

                }
            }
            ViewData["result"] = "error";
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var UserNameControl = _userRepository.Users.FirstOrDefault(u => u.username == model.UserName);
                var MailControl = _userRepository.Users.FirstOrDefault(u => u.mail == model.Mail);
                if (UserNameControl != null)
                {
                    ModelState.AddModelError("", "Bu kullanıcı adı kullanılıyor!");
                }
                else if (MailControl != null)
                {
                    ModelState.AddModelError("", "Bu mail kullanılıyor!");
                }
                else
                {
                    UTF8Encoding utf8encdng = new UTF8Encoding();
                    _userRepository.CreateUser(new User
                    {
                        Name = model.Name,
                        username = model.UserName,
                        mail = model.Mail,
                        password = SHA256.HashData(utf8encdng.GetBytes(model.Password)),
                        KayitTarihi = DateTime.Now,
                        avatarName = "default.jpg",
                        RoleId=2
                    });
                    return RedirectToAction("Login", new { result = "success" });
                }
            }

            return View(model);
        }

        [Authorize(Policy ="isLogin")]
        public IActionResult LogOutPage()
        {
            return View();
        }
 
        [Authorize(Policy ="isLogin")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );
            return Redirect("/Home/Index");
        }
 
        public IActionResult UserPage(string username)
        {
            if (username == User.FindFirstValue(ClaimTypes.Email))
            {
                return RedirectToAction("MyAccount");
            }
            else
            {
                var user = _userRepository.Users.Include(f => f.Foods.Where(c=>c.Confirmation==FoodStatus.Confirm)).ThenInclude(i => i.Images.Where(m => m.type == "main")).Include(c => c.Comments).ThenInclude(f => f.Foods).Include(l => l.Likes).ThenInclude(lf => lf.foods).ThenInclude(i => i.Images).FirstOrDefault(u => u.username == username);

                return View(user);
            }

        }


        [Authorize(Policy ="isLogin")]
        public IActionResult MyAccount()
        {
            var myaccount = _userRepository.Users.Include(r=>r.Roles).Include(f => f.Foods).ThenInclude(i => i.Images.Where(t => t.type == "main")).Include(c => c.Comments).ThenInclude(f => f.Foods).Include(l => l.Likes).ThenInclude(lf => lf.foods).ThenInclude(lfi => lfi.Images).FirstOrDefault(u => u.UserId == Convert.ToInt32(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)));
            ViewData["result"] = HttpContext.Request.Query["emailchange"];
            return View(myaccount);
        }

        [HttpPost]
        public IActionResult MyAccount(User user)
        {
            var user_me = _userRepository.Users.FirstOrDefault(u => u.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            user_me.Name = user.Name;
            user_me.username = user.username;
            _userRepository.EditUser(user_me);
            ViewData["result"] = "success";
            return View(_userRepository.Users.FirstOrDefault(u => u.UserId == user.UserId));
        }

        [Authorize(Policy ="isLogin")]
        public async Task<IActionResult> MailUpdate()
        {
            return View(new MailUpdateViewModel
            {
                CurrentMail = _userRepository.Users.FirstOrDefault(u => u.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))).mail
            });
        }

        [HttpPost]
        public IActionResult MailUpdate(MailUpdateViewModel model)
        {
            UTF8Encoding utf8encoding = new UTF8Encoding();
            var userCheck = _userRepository.Users.FirstOrDefault(u => u.mail == model.CurrentMail && u.password == SHA256.HashData(utf8encoding.GetBytes(model.Password)));
            var mailCheck = _userRepository.Users.FirstOrDefault(u => u.mail == model.NewMail);
            if (userCheck != null)
            {
                if (mailCheck == null)
                {
                    userCheck.mail = model.NewMail;
                    _userRepository.EditUser(userCheck);
                    return Redirect("/Users/MyAccount?emailchange=emailsuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Bu mail kullanılıyor!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Şifre yanlış");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfilePhoto(IFormFile profilePhoto)
        {
            string newName = Guid.NewGuid() + Path.GetExtension(profilePhoto.FileName);
            var path = Path.Join(Directory.GetCurrentDirectory(),"wwwroot/user-img",newName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                profilePhoto.CopyTo(stream);
                stream.Dispose();
                stream.Close();
            }

            var user = _userRepository.Users.FirstOrDefault(u => u.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (user.avatarName!="default.jpg") {
                System.IO.File.Delete(Path.Join(Directory.GetCurrentDirectory(),"wwwroot/user-img",user.avatarName));
            }
            
            user.avatarName=newName;
            _userRepository.EditUser(user);


            var Identity = (ClaimsIdentity)User.Identity;
            var claims = Identity.Claims.ToList();

            claims.Remove(claims.FirstOrDefault(c=>c.Type==ClaimTypes.UserData));


            claims.Add(new Claim(ClaimTypes.UserData,newName));



            var newIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var newPrincipal = new ClaimsPrincipal(newIdentity);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,newPrincipal);

            return Json(new
            {

            });
        }
    }
}