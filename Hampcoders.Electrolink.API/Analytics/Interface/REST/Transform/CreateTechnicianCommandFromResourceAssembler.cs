using Hampcoders.Electrolink.API.Analytics.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Analytics.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Analytics.Interface.REST.Transform;

public static class CreateTechnicianCommandFromResourceAssembler
{
    public static CreateTechnicianCommand ToCommandFromResource(CreateTechnicianResource resource)
    {
        return new CreateTechnicianCommand(resource.Name, resource.Email);
    }
}