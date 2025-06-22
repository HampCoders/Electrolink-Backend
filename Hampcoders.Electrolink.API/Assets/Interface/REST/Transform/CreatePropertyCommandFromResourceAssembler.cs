using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class CreatePropertyCommandFromResourceAssembler
{
    // MÉTODO CORREGIDO: ahora recibe el ownerId como un parámetro separado.
    public static CreatePropertyCommand ToCommandFromResource(CreatePropertyResource resource, Guid ownerId)
    {
        var address = new Address(
            resource.Address.Street, 
            resource.Address.Number, 
            resource.Address.City, 
            resource.Address.PostalCode, 
            resource.Address.Country,
            resource.Address.Latitude,
            resource.Address.Longitude
        );

        var region = new Region(resource.RegionName, resource.RegionCode);
        var district = new District(resource.DistrictName, resource.DistrictUbigeo);

        // CORREGIDO: Usamos el ownerId que viene del parámetro, no del resource.
        return new CreatePropertyCommand(new OwnerId(ownerId), address, region, district);
    }
}