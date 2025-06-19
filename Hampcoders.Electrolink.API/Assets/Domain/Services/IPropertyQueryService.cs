using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface IPropertyQueryService
{
    Task<Property?> Handle(GetPropertyByIdQuery query);
    Task<IEnumerable<Property>> Handle(GetAllPropertiesByOwnerIdQuery query);
    Task<IEnumerable<Property>> Handle(GetAllPropertiesQuery query);
}