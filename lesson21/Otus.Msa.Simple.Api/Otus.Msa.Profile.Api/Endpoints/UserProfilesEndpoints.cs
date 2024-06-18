using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Otus.Msa.Profile.Api.Domain;
using Otus.Msa.Profile.Api.Dto;
using Otus.Msa.Profile.Api.Infrastructure.Persistence;
using Otus.Msa.Profile.Api.Middlewares;

namespace Otus.Msa.Profile.Api.Endpoints;

/// <summary>
///		Расширения для добавления маршрутов и реализации логики эндпоинтов.
/// </summary>
public static class UserProfilesEndpoints
{
    /// <summary>
    ///		Добавляет эндпоинты создания, удаления, просмотра и обновления профиля пользователя.
    /// </summary>
    internal static IEndpointRouteBuilder MapUserProfilesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/api/v1/userprofiles");

        usersGroup.MapGet("/me", GetUserProfile);
        usersGroup.MapPut("/me", UpdateUserProfile);

        return endpoints;
    }

    /// <summary>
    ///		Получение сведений профиля пользователя.
    /// </summary>
    internal static async Task<Results<Ok<UserExtendedProfileDto>, NotFound>> GetUserProfile
        (
            UserProfileContext dbContext,
            AuthorizationContext authorizationContext,
            CancellationToken cancellationToken
        )
    {
        var userProfile = await dbContext.UserProfiles.FindAsync([authorizationContext.Id], cancellationToken: cancellationToken);
        if (userProfile is not { })
        {
            userProfile = new UserProfile() { Id = authorizationContext.Id };

            dbContext.UserProfiles.Add(userProfile);

            await dbContext.SaveChangesAsync();
        }

        var userDto = new UserExtendedProfileDto
        {
            Id = userProfile.Id,
            Login = authorizationContext.Login,
            Email = authorizationContext.Email,
            FirstName = userProfile.FirstName,
            LastName = userProfile.LastName,
            Phone = userProfile.Phone,
            City = userProfile.City
        };

        return TypedResults.Ok(userDto);
    }

    /// <summary>
    ///		Обновление сведений профиля пользователя.
    /// </summary>
    internal static async Task<Results<Ok<UserExtendedProfileDto>, NotFound>> UpdateUserProfile
    (
        [FromBody] UpdateUserProfileDto userProfileDto,
        AuthorizationContext authorizationContext,
        UserProfileContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var userProfile = await dbContext.UserProfiles.FindAsync([authorizationContext.Id], cancellationToken: cancellationToken);
        if (userProfile is not { })
        {
            userProfile = new UserProfile() { Id = authorizationContext.Id };

            dbContext.UserProfiles.Add(userProfile);
        }

        userProfile.FirstName = userProfileDto.FirstName;
        userProfile.LastName = userProfileDto.LastName;
        userProfile.Phone = userProfileDto.Phone;
        userProfile.City = userProfileDto.City;

        await dbContext.SaveChangesAsync(cancellationToken);

        var userDto = new UserExtendedProfileDto
        {
            Id = userProfile.Id,
            Login = authorizationContext.Login,
            Email = authorizationContext.Email,
            FirstName = userProfile.FirstName,
            LastName = userProfile.LastName,
            Phone = userProfile.Phone,
            City = userProfile.City
        };

        return TypedResults.Ok(userDto);
    }
}
