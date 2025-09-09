using System.ComponentModel.DataAnnotations;

namespace YemekTarifleri.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }


        [StringLength(50, MinimumLength = 6, ErrorMessage = "Kullanıcı adı minimum 6 karakter olmalıdır.")]
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez")]
        public string UserName { get; set; }
        


        [EmailAddress(ErrorMessage = "Geçerli bir mail giriniz")]
        [Required(ErrorMessage = "Mail alanı boş geçilemez")]
        public string Mail { get; set; }


        [Required(ErrorMessage = "Şifre alanı boş geçilemez")]
        [StringLength(100,MinimumLength =10,ErrorMessage ="Minimum şifre uzunluğu 10 karakterdir")]
        public string Password { get; set; }


        [Compare("Password",ErrorMessage ="Şifreler uyuşmuyor")]
        [Required(ErrorMessage = "Şifre alanı boş geçilemez")]
        public string ConfirmPassword { get; set; }
    }
}