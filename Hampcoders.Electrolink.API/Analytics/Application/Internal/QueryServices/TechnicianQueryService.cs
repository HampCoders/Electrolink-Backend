using Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.QueryServices;

public class TechnicianQueryService (ITechnicianRepository technicianRepository) : ITechnicianQueryService
{
    public async Task<Technician?> Handle(GetTechnicianByIdQuery query)
    {
        return await technicianRepository.FindByIdAsync(query.TechnicianId);
    }

    public async Task<IEnumerable<Technician>> Handle(GetAllTechniciansQuery query)
    {
        return await technicianRepository.ListAsync();
    }
}