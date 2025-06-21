using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Analytics.Interface.REST.Transform;

public static class WorkResourceFromEntityAssembler
{
    public static WorkResource ToResourceFromEntity(Work entity)
    {
        return new WorkResource(
            entity.Id,
            entity.Title,
            entity.Description,
            TechnicianResourceFromEntityAssembler.ToResourceFromEntity(entity.Technician));
    }
}