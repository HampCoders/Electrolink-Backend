using Hampcoders.Electrolink.API.Analytics.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Analytics.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Analytics.Interface.REST.Transform;

public static class AddImageAssetToWorkCommandFromResourceAssembler
{
    public static AddImageAssetToWorkCommand ToCommandFromResource(AddImageAssetToWorkResource resource, int workId)
    {
        return new AddImageAssetToWorkCommand(resource.ImageUrl, workId);
    }
}