using System.Security.Claims;

namespace Otus.Homework.Common.Authentication.Extensions;

public static class ClaimsIdentityExtensions
{
    public static int GetUserId(this ClaimsIdentity identity) => int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

    public static string GetLogin(this ClaimsIdentity identity) => identity.FindFirst(ClaimTypes.Name).Value;

    public static string GetEmail(this ClaimsIdentity identity) => identity.FindFirst(ClaimTypes.Email).Value;
}
