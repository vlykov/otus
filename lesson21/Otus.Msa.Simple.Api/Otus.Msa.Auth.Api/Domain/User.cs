namespace Otus.Msa.Auth.Api.Domain;

public class User
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}
