using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class UpdateUserViewModel
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DisplayName("First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [DisplayName("Last name")]
    public string LastName { get; set; } = string.Empty;
}
