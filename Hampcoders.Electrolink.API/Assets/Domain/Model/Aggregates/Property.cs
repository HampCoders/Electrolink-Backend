using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public partial class Property
{
    public PropertyId Id { get; private set; }
    public OwnerId OwnerId { get; private set; }
    public Address Address { get; private set; }
    public Region Region { get; private set; }
    public District District { get; private set; }
    
    public Property(OwnerId ownerId, Address address, Region region, District district) : this()
    {
        OwnerId = ownerId;
        Address = address;
        Region = region;
        District = district;
    }

    public Property(CreatePropertyCommand command)
    {
        Id = PropertyId.NewId();
        OwnerId = command.OwnerId;
        Address = command.Address;
        Region = command.Region;
        District = command.District;
        Status = EPropertyStatus.Active;
        Photo = null;
    }
    
}