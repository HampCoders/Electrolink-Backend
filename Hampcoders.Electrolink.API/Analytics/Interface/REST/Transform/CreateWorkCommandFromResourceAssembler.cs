using Hampcoders.Electrolink.API.Analytics.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Analytics.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Analytics.Interface.REST.Transform;

public static class CreateWorkCommandFromResourceAssembler
{
    public static CreateWorkCommand ToCommandFromResource(CreateWorkResource resource)
    {  
        return new CreateWorkCommand(resource.Title, resource.Description, resource.TechnicianId);
    }
}