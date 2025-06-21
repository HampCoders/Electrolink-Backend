using Hampcoders.Electrolink.API.Analytics.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;

public partial class Work 
{
    public ICollection<Asset> Assets { get;}

    public Work()
    {
        Title=string.Empty;
        Description=string.Empty;
        Assets=new List<Asset>();
    }
    
    private bool ExistsImageByUrl(string imageUrl) =>
        Assets.Any(asset =>(string)asset.GetContent() == imageUrl);
    
    public void AddImage(string imageUrl)
    {
        if (ExistsImageByUrl(imageUrl)) return;
        Assets.Add(new ImageAsset(imageUrl));
    }
    
    public void RemoveAsset(ElectrolinkAssetIdentifier identifier)
    {
        var asset = Assets.FirstOrDefault(a => a.AssetIdentifier == identifier);
        if (asset is null) return;
        Assets.Remove(asset);
    }
    
    public void ClearAssets()
    {
        Assets.Clear();
    }

    public List<ContentItem> GetContent()
    {
        var content = new List<ContentItem>();
        if (Assets.Count > 0)
            content.AddRange(Assets.Select(asset => 
                new ContentItem(asset.GetContent() as string ?? string.Empty)));
        return content;
    }
    

    public void Handle(AddImageAssetToWorkCommand command)
    {
        if(command.WorkId == Id)
            AddImage(command.ImageUrl);
    }
    
    
}