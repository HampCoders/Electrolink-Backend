using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Domain.Repositories;

public interface IPropertyRepository : IBaseRepository<Property>
{
    Task<Property?> FindByIdAsync(PropertyId id); // Versión que acepta el Value Object
    Task<IEnumerable<Property>> FindByOwnerIdAsync(OwnerId ownerId);
}