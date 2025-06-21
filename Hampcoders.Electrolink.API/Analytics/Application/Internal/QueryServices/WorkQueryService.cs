using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.QueryServices;

public class WorkQueryService (IWorkRepository workRepository) : IWorkQueryService
{
    public async Task<Work?> Handle(GetWorkByIdQuery query)
    {
        return await workRepository.FindByIdAsync(query.WorkId);
    }

    public async Task<IEnumerable<Work>> Handle(GetAllWorksQuery query)
    {
        return await workRepository.ListAsync();
    }

    public async Task<IEnumerable<Work>> Handle(GetAllWorksByTechniciansIdQuery query)
    {
        return await workRepository.FindByTechnicianIdAsync(query.TechnicianId);
    }
}