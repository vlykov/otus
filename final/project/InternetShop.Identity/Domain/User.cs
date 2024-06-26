namespace InternetShop.Identity.Domain;

public class User
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
}
