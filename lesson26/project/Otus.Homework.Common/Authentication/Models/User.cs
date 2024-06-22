using System.Security.Claims;
using Otus.Homework.Common.Authentication.Extensions;

namespace Otus.Homework.Common.Authentication.Models;

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
