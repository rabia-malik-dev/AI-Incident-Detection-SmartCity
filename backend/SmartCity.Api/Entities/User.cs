namespace SmartCity.Api.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Role { get; set; } = "Admin";
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<Video> Videos { get; set; } = [];
}
