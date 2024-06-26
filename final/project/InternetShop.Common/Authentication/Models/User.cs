using System.Security.Claims;
using InternetShop.Common.Authentication.Extensions;

namespace InternetShop.Common.Authentication.Models;

public class User
{
    public User(ClaimsIdentity identity)
    {
        ArgumentNullException.ThrowIfNull(identity, nameof(identity));

        Id = identity.GetUserId();
        Login = identity.GetLogin();
        Email = identity.GetEmail();
    }

    public int Id { get; private set; }

    public string Login { get; private set; }

    public string Email { get; private set; }
}
