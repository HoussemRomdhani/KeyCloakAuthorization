using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class RegisterViewModel
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [MinLength(8, ErrorMessage = "Password must be at least 6 characters long.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
      ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DisplayName("First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [DisplayName("Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Select Role")]
    public string SelectedRole { get; set; } = string.Empty;

    public List<SelectListItem> Roles { get; set; } = new();
}
