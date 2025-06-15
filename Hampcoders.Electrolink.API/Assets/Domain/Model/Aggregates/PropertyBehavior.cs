using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public partial class Property
{
    public EPropertyStatus Status { get; private set; }
    private readonly List<PropertyPhoto> _photos = new();
    public IReadOnlyCollection<PropertyPhoto> Photos => _photos.AsReadOnly();

    public Property()
    {
        Status = EPropertyStatus.Active;
        _photos = new List<PropertyPhoto>();
    }

    public void AddPhoto(string photoUrl)
    {
        if (_photos.Any(p => p.PhotoURL == photoUrl)) return;
        _photos.Add(new PropertyPhoto(photoUrl));
    }
    
    private void UpdateAddress(Address newAddress)
    {
        if (Status == EPropertyStatus.Inactive)
            throw new InvalidOperationException("Cannot update the address of an inactive property.");
        Address = newAddress;
    }
    
    private void Deactivate()
    {
        if (Status == EPropertyStatus.Inactive) return;
        Status = EPropertyStatus.Inactive;
    }

    public void Handle(AddPhotoToPropertyCommand command)
    {
        if(command.Id == Id)
            AddPhoto(command.PhotoUrl);
    }
    
    public void Handle(UpdatePropertyAddressCommand command)
    {
        if (command.Id == Id)
            UpdateAddress(command.NewAddress);
    }
    
    public void Handle(DeactivatePropertyCommand command)
    {
        if (command.Id == Id)
            Deactivate();
    }
}