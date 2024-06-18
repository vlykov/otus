using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Otus.Msa.Auth.Api.Constants;
using Otus.Msa.Auth.Api.Domain;
using Otus.Msa.Auth.Api.Dto;
using Otus.Msa.Auth.Api.Infrastructure.Persistence;

namespace Otus.Msa.Auth.Api.Endpoints;

/// <summary>
///		Расширения для добавления маршрутов и реализации логики эндпоинтов.
/// </summary>
public static class UsersEndpoints
{
    /// <summary>
    ///		Добавляет эндпоинты созданию, удалению, просмотру и обновлению пользователей.
    /// </summary>
    internal static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/api/v1/users");

        usersGroup.MapPost("/register", Register);
        usersGroup.MapPost("/login", Login);
        usersGroup.MapGet("/signin", SignIn);
        usersGroup.MapGet("/auth", Auth);
        usersGroup.MapPost("/logout", Logout);

        return endpoints;
    }

    /// <summary>
    ///		Регистрация пользователя.
    /// </summary>
    internal static async Task<Created<UserDto>> Register
        (
            [FromBody] RegisterUserDto registerUserDto,
            UserContext dbContext,
            CancellationToken cancellationToken
        )
    {
        // todo: add validation and exception processing
        var hashedPassword = CalculateMD5(registerUserDto.Password);

        var user = new User
        {
            Login = registerUserDto.Login,
            Password = hashedPassword,
            Email = registerUserDto.Email
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        var newUser = new UserDto
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email
        };

        return TypedResults.Created($"/api/v1/users/{user.Id}", newUser);
    }

    // todo: add autoclear on timeout
    private static readonly Dictionary<Guid, UserDto> UserSessions = [];

    /// <summary>
    ///		Вход пользователя.
    /// </summary>
    internal static async Task<Results<Ok, UnauthorizedHttpResult>> Login
        (
            [FromBody] LoginUserDto loginUserDto,
            HttpContext httpContext,
            UserContext dbContext,
            CancellationToken cancellationToken
        )
    {
        if (string.IsNullOrWhiteSpace(loginUserDto.Login) ||
            string.IsNullOrWhiteSpace(loginUserDto.Password))
        {
            return TypedResults.Unauthorized();
        }

        var hashedPassword = CalculateMD5(loginUserDto.Password);
        if (await dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Login == loginUserDto.Login && user.Password == hashedPassword, cancellationToken: cancellationToken) is User user)
        {
            var sessionId = Guid.NewGuid();
            UserSessions[sessionId] = new UserDto
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
            };

            httpContext.Response.Cookies.Append("session_id", $"sakurlyk_{sessionId}",
                new CookieOptions
                {
                    HttpOnly = false,
                    MaxAge = TimeSpan.FromDays(5)
                });

            return TypedResults.Ok();
        }

        return TypedResults.Unauthorized();
    }

    internal static IResult SignIn() => Results.Ok(new { Message = "Please go to login and provide Login/Password" });

    /// <summary>
    ///		Проверка того, что пользователь аутентифицирован.
    /// </summary>
    internal static Results<Ok<UserDto>, UnauthorizedHttpResult> Auth
        (
            HttpContext httpContext,
            CancellationToken cancellationToken
        )
    {
        var cookies = httpContext.Request.Cookies["session_id"];

        if (cookies is { } && 
            cookies.Split('_') is { Length: 2 } splitted  && 
            Guid.TryParse(splitted[1], out var session_id))
        {
            var user = UserSessions[session_id];

            if (user is { })
            {
                httpContext.Response.Headers.Append(CustomHeaders.AuthorizationUserId, user.Id.ToString());
                httpContext.Response.Headers.Append(CustomHeaders.AuthorizationUserLogin, user.Login);
                httpContext.Response.Headers.Append(CustomHeaders.AuthorizationUserEmail, user.Email);

                return TypedResults.Ok(user);
            }
        }

        return TypedResults.Unauthorized();
    }

    /// <summary>
    ///		Выход пользователя.
    /// </summary>
    internal static Ok Logout
    (
        HttpContext httpContext,
        CancellationToken cancellationToken
    )
    {
        httpContext.Response.Cookies.Delete("session_id");
        return TypedResults.Ok();
    }

    private static string CalculateMD5(string text) => Convert.ToHexString(MD5.HashData(Encoding.ASCII.GetBytes(text ?? "")));
}
