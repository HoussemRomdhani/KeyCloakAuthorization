namespace WebApp.ViewModels;

public class BookViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Quantity { get; set; } 
    public int Year { get; set; }
    public bool CanPurchase => Quantity > 0;
}


