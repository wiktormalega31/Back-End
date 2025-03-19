namespace LoginAPI.Models;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime Created { get; set; }
}
