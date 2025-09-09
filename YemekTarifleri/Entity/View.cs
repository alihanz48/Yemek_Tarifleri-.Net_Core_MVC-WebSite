namespace YemekTarifleri.Entity;

public class View
{
    public int viewId { get; set; }
    public string EnterDetail { get; set; }
    public int FoodId { get; set; }
    public Food foods { get; set; } = null!;
    public DateTime time { get; set; }
}