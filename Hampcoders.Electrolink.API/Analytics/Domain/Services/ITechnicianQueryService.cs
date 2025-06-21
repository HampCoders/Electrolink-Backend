using Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Services;

public interface ITechnicianQueryService
{
    Task<Technician?> Handle(GetTechnicianByIdQuery query);
    
    Task<IEnumerable<Technician>> Handle(GetAllTechniciansQuery query);
    
}