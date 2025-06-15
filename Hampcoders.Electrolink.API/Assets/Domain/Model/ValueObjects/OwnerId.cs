namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

/// <summary>
/// Represents the unique identifier for an owner.
/// </summary>
public record OwnerId
{
    private string Id { get; }
    public OwnerId(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Owner ID cannot be null or whitespace.", nameof(id));
        }
        Id = id;
    }
    
    public static implicit operator string(OwnerId ownerId) => ownerId.Id;
}