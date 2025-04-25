namespace Haihv.Elis.Tool.TraCuuGcn.Models;

public class UserInfo
{
    public string Name { get; set; } = string.Empty;
    public string PreferredUsername { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public bool IsLocalAccount { get; set; }
    public List<string> Roles { get; set; } = [];
       
}