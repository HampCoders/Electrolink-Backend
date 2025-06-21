using Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Analytics.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Analytics.Interface.REST.Transform;

public static class TechnicianResourceFromEntityAssembler
{
    public static TechnicianResource ToResourceFromEntity(Technician entity)
    {
        return new TechnicianResource(entity.Id, entity.Name, entity.Email);
    }
}