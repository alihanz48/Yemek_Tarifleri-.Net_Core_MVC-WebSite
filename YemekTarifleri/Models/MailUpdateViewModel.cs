namespace YemekTarifleri.Models
{
    public class MailUpdateViewModel
    {
        public int UserId { get; set; }
        public string CurrentMail { get; set; }
        public string NewMail { get; set; }
        public string Password { get; set; }
    }
}