namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;

public class ImageAsset:Asset
{
    public Uri? ImageUri { get; }
 
    public ImageAsset()
    {
        ImageUri = null;
    }

    public ImageAsset(string imageUrl)
    {
        ImageUri = new Uri(imageUrl);
    }
    public override string GetContent()
    {
        return ImageUri != null ? ImageUri.AbsoluteUri : string.Empty;
    }
}