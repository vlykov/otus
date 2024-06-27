using InternetShop.Common.Authentication.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace InternetShop.Common.Authentication;

public class CustomHeadersAuthenticationOptions : AuthenticationSchemeOptions
{
}

public class CustomHeadersAuthenticationHandler : AuthenticationHandler<CustomHeadersAuthenticationOptions>
{
    public CustomHeadersAuthenticationHandler(IOptionsMonitor<CustomHeadersAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Headers.TryGetValue(CustomHeaders.AuthorizationUserId, out var headerUserId);
        Request.Headers.TryGetValue(CustomHeaders.AuthorizationUserLogin, out var headerUserLogin);
        Request.Headers.TryGetValue(CustomHeaders.AuthorizationUserEmail, out var headerUserEmail);

        var userId = headerUserId.FirstOrDefault();
        var userLogin = headerUserLogin.FirstOrDefault();
        var userEmail = headerUserEmail.FirstOrDefault();

        userId = "1";
        userLogin = "some_login";
        userEmail = "some@email.com";

        if (string.IsNullOrWhiteSpace(userId)
            || !int.TryParse(userId, out var _)
            || string.IsNullOrWhiteSpace(userLogin)
            || string.IsNullOrWhiteSpace(userEmail))
        {
            return Task.FromResult(AuthenticateResult.Fail("Значение идентификатора пользователя некорректное или незадано"));
        }

        try
        {
            return Task.FromResult(AssignUserClaims(userId, userLogin, userEmail));
        }
        catch (Exception ex)
        {
            return Task.FromResult(AuthenticateResult.Fail(ex.Message));
        }
    }

    private AuthenticateResult AssignUserClaims(string userId, string userLogin, string userEmail)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userLogin),
            new Claim(ClaimTypes.Email, userEmail)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new System.Security.Principal.GenericPrincipal(identity, null);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
