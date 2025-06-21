using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Services;

public interface IWorkQueryService
{


    Task<Work> Handle(GetWorkByIdQuery query);
    
    Task<IEnumerable<Work>> Handle(GetAllWorksQuery query);
    
    Task<IEnumerable<Work>> Handle(GetAllWorksByTechniciansIdQuery query); 
    
}