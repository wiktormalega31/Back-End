namespace MyApi.Models;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public required string Password { get; set; }
    public DateTime Created { get; set; }
}
