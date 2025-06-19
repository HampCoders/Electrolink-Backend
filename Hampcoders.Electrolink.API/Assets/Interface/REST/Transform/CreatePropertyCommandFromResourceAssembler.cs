using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class CreatePropertyCommandFromResourceAssembler
{
    // Convierte el Resource de la API en un Command para el Dominio
    public static CreatePropertyCommand ToCommandFromResource(CreatePropertyResource resource)
    {
        // Ahora la creación coincide con la definición del record Address
        var address = new Address(
            resource.Address.Street, 
            resource.Address.Number, 
            resource.Address.City, 
            resource.Address.PostalCode, 
            resource.Address.Country,
            resource.Address.Latitude,
            resource.Address.Longitude
        );
        var region = new Region(resource.RegionName, "PE-LMA");
        var district = new District(resource.DistrictName, "150101");

        return new CreatePropertyCommand(new OwnerId(resource.OwnerId), address, region, district);
    }
}