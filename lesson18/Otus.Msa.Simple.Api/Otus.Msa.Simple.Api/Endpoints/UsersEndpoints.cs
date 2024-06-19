using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Otus.Msa.Simple.Api.Domain;
using Otus.Msa.Simple.Api.Dto;
using Otus.Msa.Simple.Api.Infrastructure.Persistence;

namespace Otus.Msa.Simple.Api.Endpoints;

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

        usersGroup.MapPost("/", CreateUser);
        usersGroup.MapGet("/{userId}", GetUser);
        usersGroup.MapDelete("/{userId}", DeleteUser);
        usersGroup.MapPut("/{userId}", UpdateUser);

        return endpoints;
    }

    /// <summary>
    ///		Создание пользователя.
    /// </summary>
    internal static async Task<Created<UserDto>> CreateUser
        (
            [FromBody] CreateUserDto userDto,
            UserContext dbContext,
            CancellationToken cancellationToken
        )
    {
        var user = new User
        {
            UserName = userDto.UserName,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Phone = userDto.Phone
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        var newUser = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
        };

        return TypedResults.Created($"/api/v1/users/{user.Id}", newUser);
    }

    /// <summary>
    ///		Получение сведений о пользователе.
    /// </summary>
    internal static async Task<Results<Ok<UserDto>, NotFound>> GetUser
        (
            [FromRoute] int userId,
            UserContext dbContext,
            CancellationToken cancellationToken
        )
    {
        if (await dbContext.Users.FindAsync([userId], cancellationToken: cancellationToken) is User user)
        {
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
            };

            return TypedResults.Ok(userDto);
        }

        return TypedResults.NotFound();
    }

    /// <summary>
    ///		Удаление пользователя.
    /// </summary>
    internal static async Task<Results<NoContent, NotFound>> DeleteUser
        (
            [FromRoute] int userId,
            UserContext dbContext,
            CancellationToken cancellationToken
        )
    {
        if (await dbContext.Users.FindAsync([userId], cancellationToken: cancellationToken) is User user)
        {
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync(cancellationToken);
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    /// <summary>
    ///		Обновление сведений о пользователе.
    /// </summary>
    internal static async Task<Results<NoContent, NotFound>> UpdateUser
    (
        [FromRoute] int userId,
        [FromBody] UpdateUserDto userDto,
        UserContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var user = await dbContext.Users.FindAsync(userId);

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        user.UserName = userDto.UserName;
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Email = userDto.Email;
        user.Phone = userDto.Phone;

        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
