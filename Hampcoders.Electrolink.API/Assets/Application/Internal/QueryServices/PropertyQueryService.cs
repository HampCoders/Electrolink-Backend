using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;

public class PropertyQueryService(IPropertyRepository propertyRepository) : IPropertyQueryService
{
    /// <summary>
    /// Maneja el query para obtener una propiedad por su ID.
    /// </summary>
    public async Task<Property?> Handle(GetPropertyByIdQuery query)
    {
        // Llama al método personalizado del repositorio que acepta el Value Object.
        return await propertyRepository.FindByIdAsync(new PropertyId(query.PropertyId));
    }

    /// <summary>
    /// Maneja el query para obtener todas las propiedades de un propietario.
    /// Corresponde directamente a la tarea técnica TS-02.
    /// </summary>
    public async Task<IEnumerable<Property>> Handle(GetAllPropertiesByOwnerIdQuery query)
    {
        return await propertyRepository.FindByOwnerIdAsync(new OwnerId(Guid.Parse(query.OwnerId)));
    }

    /// <summary>
    /// Maneja el query para obtener todas las propiedades del sistema.
    /// </summary>
    public async Task<IEnumerable<Property>> Handle(GetAllPropertiesQuery query)
    {
        return await propertyRepository.ListAsync();
    }
}