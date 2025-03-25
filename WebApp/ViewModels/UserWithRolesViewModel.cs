namespace WebApp.ViewModels;

public class UserWithRolesViewModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Roles { get; set; } = string.Empty;
    public bool CanAssignRole { get; set; }
    public bool CanUnAssignRole { get; set; }
}
