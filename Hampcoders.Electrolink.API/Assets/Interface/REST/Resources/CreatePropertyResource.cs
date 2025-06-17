namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record CreatePropertyResource(Guid OwnerId, AddressResource Address, string RegionName, string DistrictName);
