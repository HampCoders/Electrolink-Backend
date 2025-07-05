
using System.Net.Mime;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Profile Endpoints.")]
public class ProfilesController(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService)
: ControllerBase
{
    [HttpPost]
    [SwaggerOperation("Create Profile", "Create a new profile (Homeowner or Technician).", OperationId = "CreateProfile")]
    [SwaggerResponse(201, "The profile was created.", typeof(ProfileResource))]
    [SwaggerResponse(400, "The profile was not created.")]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileResource resource)
    {
        var command = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
        var profile = await profileCommandService.Handle(command);
        if (profile is null) return BadRequest();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return CreatedAtAction(nameof(GetProfileById), new { profileId = profile.Id }, profileResource);
    }

    [HttpGet("{profileId:int}")]
    [SwaggerOperation("Get Profile by Id", "Get a profile by its unique identifier.", OperationId = "GetProfileById")]
    [SwaggerResponse(200, "The profile was found and returned.", typeof(ProfileResource))]
    [SwaggerResponse(404, "The profile was not found.")]
    public async Task<IActionResult> GetProfileById(int profileId)
    {
        var query = new GetProfileByIdQuery(profileId);
        var profile = await profileQueryService.Handle(query);
        if (profile is null) return NotFound();
        var resource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(resource);
    }

    [HttpGet]
    [SwaggerOperation("Get All Profiles", "Get all profiles.", OperationId = "GetAllProfiles")]
    [SwaggerResponse(200, "The profiles were found and returned.", typeof(IEnumerable<ProfileResource>))]
    public async Task<IActionResult> GetAllProfiles()
    {
        var query = new GetAllProfilesQuery();
        var profiles = await profileQueryService.Handle(query);
        var resources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("by-role/{role}")]
    [SwaggerOperation("Get Profiles by Role", "Get all profiles filtered by role.", OperationId = "GetProfilesByRole")]
    [SwaggerResponse(200, "Profiles with the specified role returned.", typeof(IEnumerable<ProfileResource>))]
    [SwaggerResponse(400, "Invalid role.")]
    public async Task<IActionResult> GetProfilesByRole(string role)
    {
        if (!Enum.TryParse<Role>(role, true, out var parsedRole))
            return BadRequest("Invalid role.");

        var query = new GetProfilesByRoleQuery(parsedRole);
        var profiles = await profileQueryService.Handle(query);
        var resources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("by-email")]
    [SwaggerOperation("Get Profile by Email", "Get a profile by its email address.", OperationId = "GetProfileByEmail")]
    [SwaggerResponse(200, "The profile was found and returned.", typeof(ProfileResource))]
    [SwaggerResponse(404, "No profile found with that email.")]
    public async Task<IActionResult> GetProfileByEmail([FromQuery] string email)
    {
      var query = new GetProfileByEmailQuery(new EmailAddress(email));
        var profile = await profileQueryService.Handle(query);
        if (profile is null) return NotFound();
        var resource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(resource);
    }
}


