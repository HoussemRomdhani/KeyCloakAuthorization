using System.ComponentModel.DataAnnotations;
using WebApp.Constants;

namespace WebApp.ViewModels;

public class CreateBookViewModel
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Range(minimum: 1, maximum: 100)]
    public int Quantity { get; set; } = 1;

    [Required]
    [Range(minimum: BookYearRange.MIN, maximum: BookYearRange.MAX)]
    public int Year { get; set; }
}


