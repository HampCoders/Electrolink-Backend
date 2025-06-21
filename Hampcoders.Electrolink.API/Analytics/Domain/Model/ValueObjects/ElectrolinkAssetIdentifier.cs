namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record ElectrolinkAssetIdentifier(Guid Identifier)
{

    public ElectrolinkAssetIdentifier() : this(Guid.NewGuid()){}

}