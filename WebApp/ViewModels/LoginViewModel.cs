namespace WebApp.ViewModels;

using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
