
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Profiles.Application.ACL;

/// <summary>
/// Facade for the profiles context
/// </summary>
public class ProfilesContextFacade(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService
) : IProfilesContextFacade
{
    public async Task<int> CreateProfile(
        int userId,
        string firstName,
        string lastName,
        string email,
        string street,
        string number,
        string city,
        string postalCode,
        string country,
        string role,
        string? preferredContactTime,
        string? dni,
        string? licenseNumber,
        string? specialization)
    {
        // Validar rol
        var parsedRole = Enum.TryParse<Role>(role, true, out var finalRole) ? finalRole : Role.HomeOwner;

        var createProfileCommand = new CreateProfileCommand(
            firstName,
            lastName,
            email,
            street,
            number,
            city,
            postalCode,
            country,
            finalRole,
            preferredContactTime,
            dni,
            licenseNumber,
            specialization
        );

        var profile = await profileCommandService.Handle(createProfileCommand);
        return profile?.Id ?? 0;
    }

    public async Task<int> FetchProfileIdByEmail(string email)
    {
        var getProfileByEmailQuery = new GetProfileByEmailQuery(new EmailAddress(email));
        var profile = await profileQueryService.Handle(getProfileByEmailQuery);
        return profile?.Id ?? 0;
    }
}