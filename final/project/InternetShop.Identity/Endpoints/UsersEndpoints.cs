using System.Security.Cryptography;
using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShop.Common.Authentication.Constants;
using InternetShop.Identity.Domain;
using InternetShop.Identity.Dto;
using InternetShop.Identity.Extensions;
using InternetShop.Identity.Infrastructure.Persistence;
using static InternetShop.Common.Contracts.MessageBroker.Events.Identity;
using InternetShop.Common.ExceptionHandlers.Models;
using System.Linq;

namespace InternetShop.Identity.Endpoints;

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
        var usersGroup = endpoints.MapGroup("/users");

        usersGroup.MapPost("/register", Register);
        usersGroup.MapPost("/login", Login);
        usersGroup.MapPost("/logout", Logout);
        usersGroup.MapGet("/signin", SignIn);
        usersGroup.MapGet("/auth", Auth);

        return endpoints;
    }

    /// <summary>
    ///		Регистрация пользователя.
    /// </summary>
    internal static async Task<Results<Ok<UserDto>, BadRequest<ErrorResponse>>> Register
        (
            [FromBody] RegisterUserDto registerUserDto,
            CoreDbContext dbContext,
            IBus bus,
            CancellationToken cancellationToken
        )
    {
        // todo: add validation and exception processing
        var hashedPassword = CalculateMD5(registerUserDto.Password);

        if (await dbContext.Users.AnyAsync(user => user.Login == registerUserDto.Login || user.Email == registerUserDto.Email, cancellationToken))
        {
            return TypedResults.BadRequest(new ErrorResponse("Пользователь уже существует"));
        }

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

        await bus.Publish(new UserRegistered(user.Id), cancellationToken);

        return TypedResults.Ok(newUser);
    }

    /// <summary>
    ///		Вход пользователя.
    /// </summary>
    internal static async Task<Results<Ok, UnauthorizedHttpResult>> Login
        (
            [FromBody] LoginUserDto loginUserDto,
            HttpContext httpContext,
            CoreDbContext dbContext,
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
            httpContext.Session.Set("user", new UserDto
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
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
        var user = httpContext.Session.Get<UserDto>("user");

        if (user is { })
        {
            httpContext.Response.Headers.Append(CustomHeaders.AuthorizationUserId, user.Id.ToString());
            httpContext.Response.Headers.Append(CustomHeaders.AuthorizationUserLogin, user.Login);
            httpContext.Response.Headers.Append(CustomHeaders.AuthorizationUserEmail, user.Email);

            return TypedResults.Ok(user);
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
        httpContext.Session.Remove("user");

        return TypedResults.Ok();
    }

    private static string CalculateMD5(string text) => Convert.ToHexString(MD5.HashData(Encoding.ASCII.GetBytes(text ?? "")));
}
