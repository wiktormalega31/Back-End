namespace MyApi.Models;

public class UserLoginRequest
{
    public required string Name { get; set; }
    public required string Password { get; set; }
}
