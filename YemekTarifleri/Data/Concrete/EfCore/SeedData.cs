using System.Runtime.CompilerServices;
using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Data.Concrete.EfCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

namespace YemekTarifleri.Entity
{
    public class SeedData
    {
        public static void TestVerileriDoldur(IApplicationBuilder app)
        {
            var _context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<YemekTarifleriContext>();

            if (_context != null)
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }

                if (!_context.Roles.Any())
                {
                    _context.Roles.AddRange(new Role
                    {
                        RoleId = 1,
                        RoleName = "admin"
                    }, new Role
                    {
                        RoleId = 2,
                        RoleName = "user"
                    });
                }

                if (!_context.Users.Any())
                {
                    UTF8Encoding utf8encoding = new UTF8Encoding();
                    _context.Users.AddRange(
                       new User { Name = "Gökhan Güler", username = "gokhans48", mail = "gokhans@gmail.com", password = SHA256.HashData(utf8encoding.GetBytes("123456789")), KayitTarihi = DateTime.Now.AddDays(-10), avatarName = "gokhans48.jpg", RoleId = 1 },
                       new User { Name = "Ahmet Uçar", username = "ahmet+", mail = "ahmetucar@gmail.com", password = SHA256.HashData(utf8encoding.GetBytes("ASD123ASD")), KayitTarihi = DateTime.Now.AddDays(-15), avatarName = "ahmet+.jpg", RoleId = 2 },
                       new User { Name = "Melike Göksu", username = "mgoksu", mail = "melikegoksu@gmail.com", password = SHA256.HashData(utf8encoding.GetBytes("asd1asd")), KayitTarihi = DateTime.Now.AddDays(-5), avatarName = "mgoksu.jpg", RoleId = 2 }
                    );
                    _context.SaveChanges();
                }

                if (!_context.Ftypes.Any())
                {
                    _context.Ftypes.AddRange(
                        new Ftype { Name = "Ana Yemekler", Url = "ana-yemekler" },
                        new Ftype { Name = "Vegan", Url = "vegan" },
                        new Ftype { Name = "Kahvaltı", Url = "kahvalti" },
                        new Ftype { Name = "Tatlılar", Url = "tatlilar" }
                    );
                    _context.SaveChanges();
                }

                if (!_context.Foods.Any())
                {

                    _context.Foods.AddRange(
                        new Food { isim = "Mantı", aciklama = "Yoğurtlu Kıymalı Mantı", tarih = Convert.ToDateTime("05/05/2025"), url = "manti", Ftypes = _context.Ftypes.Take(1).ToList(), UserID = 1, Confirmation = FoodStatus.Confirm },
                        new Food { isim = "Yoğurtlama", aciklama = "Kızartmalı biberli yoğurtlama", tarih = Convert.ToDateTime("08/05/2025"), url = "yogurtlama", Ftypes = _context.Ftypes.Take(2).ToList(), UserID = 1, Confirmation = FoodStatus.Confirm },
                        new Food { isim = "Nohut Kapama", aciklama = "salçalı nohut kapama tarifi", tarih = Convert.ToDateTime("15/04/2025"), url = "nohut-kapama", Ftypes = _context.Ftypes.Take(3).ToList(), UserID = 2, Confirmation = FoodStatus.Confirm },
                        new Food { isim = "Patlıcan Musakka", aciklama = "Leziz patlıcan musakka", tarih = Convert.ToDateTime("20/04/2025"), url = "patlican-musakka", Ftypes = _context.Ftypes.Where(i => i.TypeID == 4).ToList(), UserID = 3, Confirmation = FoodStatus.Confirm }
                    );
                    _context.SaveChanges();
                }

                if (!_context.Steps.Any())
                {
                    _context.Steps.AddRange(
                        new Step { Text = "Mantıları kaba alalım", FoodID = 1 },
                        new Step { Text = "Mantıları kaynatalım", FoodID = 1 },
                        new Step { Text = "Mantıları yoğurtlayalım", FoodID = 1 },
                        new Step { Text = "Afiyet Olsun", FoodID = 1 },
                        new Step { Text = "Yoğurdu kaba dökelim", FoodID = 2 },
                        new Step { Text = "Patatesleri ekleyelim", FoodID = 2 },
                        new Step { Text = "Karıştıralım", FoodID = 2 },
                        new Step { Text = "Afiyet Olsun", FoodID = 2 },
                        new Step { Text = "Nohutları pişirelim", FoodID = 3 },
                        new Step { Text = "Nohutu salçalayalım", FoodID = 3 },
                        new Step { Text = "Afiyet Olsun", FoodID = 3 },
                        new Step { Text = "Patlıcanları Koyalım", FoodID = 4 },
                        new Step { Text = "Kıymaları dolduralım", FoodID = 4 },
                        new Step { Text = "Afiyet Olsun", FoodID = 4 }
                    );
                    _context.SaveChanges();
                }

                if (!_context.Ingredients.Any())
                {
                    _context.Ingredients.AddRange(
                        new Ingredient { Text = "Mantı", FoodID = 1 },
                        new Ingredient { Text = "Yoğurt", FoodID = 1 },
                        new Ingredient { Text = "Kıyma", FoodID = 1 },
                        new Ingredient { Text = "Patates", FoodID = 2 },
                        new Ingredient { Text = "Yoğurt", FoodID = 2 },
                        new Ingredient { Text = "Nohut", FoodID = 3 },
                        new Ingredient { Text = "Salça", FoodID = 3 },
                        new Ingredient { Text = "Domates", FoodID = 3 },
                        new Ingredient { Text = "Yumurta", FoodID = 3 },
                        new Ingredient { Text = "Patlıcan", FoodID = 4 },
                        new Ingredient { Text = "Kıyma", FoodID = 4 },
                        new Ingredient { Text = "Salça", FoodID = 4 }
                    );
                    _context.SaveChanges();
                }

                if (!_context.Images.Any())
                {
                    _context.Images.AddRange(
                        new Image { name = "m1.jpg", type = "main", FoodId = 1 },
                        new Image { name = "m2.jpeg", type = "normal", FoodId = 1 },
                        new Image { name = "m3.jpeg", type = "normal", FoodId = 1 },
                        new Image { name = "m4.jpeg", type = "normal", FoodId = 1 },
                        new Image { name = "m5.jpeg", type = "normal", FoodId = 1 },
                        new Image { name = "y1.jpeg", type = "main", FoodId = 2 },
                        new Image { name = "y2.jpeg", type = "normal", FoodId = 2 },
                        new Image { name = "y3.jpeg", type = "normal", FoodId = 2 },
                        new Image { name = "n1.jpeg", type = "main", FoodId = 3 },
                        new Image { name = "n2.jpeg", type = "normal", FoodId = 3 },
                        new Image { name = "n3.jpeg", type = "normal", FoodId = 3 },
                        new Image { name = "k1.jpeg", type = "main", FoodId = 4 },
                        new Image { name = "k2.jpeg", type = "normal", FoodId = 4 },
                        new Image { name = "k3.jpeg", type = "normal", FoodId = 4 },
                        new Image { name = "k4.jpeg", type = "normal", FoodId = 4 }
                    );
                    _context.SaveChanges();
                }



                if (!_context.Comments.Any())
                {
                    _context.Comments.AddRange(
                        new Comment { Text = "Harika", PublishDate = DateTime.Now.AddHours(-5), UserID = 1, FoodID = 1 },
                        new Comment { Text = "Çok Güzel", PublishDate = DateTime.Now.AddDays(-10), UserID = 1, FoodID = 2 },
                        new Comment { Text = "Bu 5 kişilik!", PublishDate = DateTime.Now.AddHours(-15), UserID = 1, FoodID = 3 },
                        new Comment { Text = "Mükemmel", PublishDate = DateTime.Now.AddHours(-8), UserID = 2, FoodID = 1 },
                        new Comment { Text = "Nefis", PublishDate = DateTime.Now.AddHours(-9), UserID = 2, FoodID = 2 },
                        new Comment { Text = "Afiyet Olsun!", PublishDate = DateTime.Now.AddHours(-11), UserID = 2, FoodID = 3 },
                        new Comment { Text = "Ağzınıza layık bir tat", PublishDate = DateTime.Now.AddHours(-12), UserID = 3, FoodID = 1 },
                        new Comment { Text = "Teşekkürler", PublishDate = DateTime.Now.AddHours(-13), UserID = 3, FoodID = 2 },
                        new Comment { Text = "Deneyeceğim", PublishDate = DateTime.Now.AddHours(-14), UserID = 3, FoodID = 3 },
                        new Comment { Text = "Tuzu biraz fazla atın", PublishDate = DateTime.Now.AddHours(-15), UserID = 3, FoodID = 4 },
                        new Comment { Text = "Patlıcanlar büyük olmalı!", PublishDate = DateTime.Now.AddHours(-5), UserID = 2, FoodID = 4 }
                    );
                    _context.SaveChanges();
                }

                if (!_context.Likes.Any())
                {
                    _context.Likes.AddRange(
                        new Like { foodId = 4, userId = 1 },
                        new Like { foodId = 2, userId = 1 },
                        new Like { foodId = 1, userId = 1 },
                        new Like { foodId = 1, userId = 2 },
                        new Like { foodId = 4, userId = 2 },
                        new Like { foodId = 2, userId = 2 },
                        new Like { foodId = 3, userId = 3 },
                        new Like { foodId = 2, userId = 3 }
                    );
                    _context.SaveChanges();
                }

                if (!_context.Views.Any())
                {
                    _context.Views.AddRange(
                        new View{EnterDetail="Desktop-10",FoodId=1,time=DateTime.Now.AddDays(10)},
                        new View{EnterDetail="Desktop-9",FoodId=1,time=DateTime.Now.AddDays(2)},
                        new View{EnterDetail="Desktop-8",FoodId=1,time=DateTime.Now.AddDays(6)},
                        new View{EnterDetail="Desktop-7",FoodId=2,time=DateTime.Now.AddDays(5)},
                        new View{EnterDetail="gokhans@gmail.com",FoodId=2,time=DateTime.Now.AddDays(7)},
                        new View{EnterDetail="gokhans@gmail.com",FoodId=3,time=DateTime.Now.AddDays(3)},
                        new View{EnterDetail="melikegoksu@gmail.com",FoodId=3,time=DateTime.Now.AddDays(2)},
                        new View{EnterDetail="ahmetucar@gmail.com",FoodId=3,time=DateTime.Now.AddDays(5)},
                        new View{EnterDetail="Desktop-2",FoodId=4,time=DateTime.Now.AddDays(9)},
                        new View{EnterDetail="Desktop-1",FoodId=4,time=DateTime.Now.AddDays(8)}
                    );
                    _context.SaveChanges();
                }

                
            }
        }
    }
}