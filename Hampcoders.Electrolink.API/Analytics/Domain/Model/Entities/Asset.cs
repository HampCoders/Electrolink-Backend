using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;

public partial class Asset
{
    public int Id { get; }
    public ElectrolinkAssetIdentifier AssetIdentifier { get; private set; } = new();
    
    public virtual object GetContent()
    {
        return string.Empty;
    }
}