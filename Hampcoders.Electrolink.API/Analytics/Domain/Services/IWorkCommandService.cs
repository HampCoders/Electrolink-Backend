using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Services;

public interface IWorkCommandService
{
    Task<Work?>Handle(AddImageAssetToWorkCommand command);
    
    Task<Work?> Handle(CreateWorkCommand command);
    
}