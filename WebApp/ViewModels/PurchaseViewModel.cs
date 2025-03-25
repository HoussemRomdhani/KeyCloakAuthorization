using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class PurchaseViewModel
{
    public int BookId { get; set; }

    public string BookTitle { get; set; } = string.Empty;

    [Required]
    [Range(1, 100, ErrorMessage = "Quantity must be at least 1 and not exceed 100.")]
    public int Quantity { get; set; } = 1;
}
