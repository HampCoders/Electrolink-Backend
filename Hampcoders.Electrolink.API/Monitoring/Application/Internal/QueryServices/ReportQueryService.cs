using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Queries;
using Hamcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hamcoders.Electrolink.API.Monitoring.Domain.Services;

namespace Hamcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;

public class ReportQueryService : IReportQueryService
{
    private readonly IReportRepository _repository;

    public ReportQueryService(IReportRepository repository)
    {
        _repository = repository;
    }

    public async Task<Report?> GetByRequestIdAsync(Guid requestId)
    {
        return await _repository.GetByRequestIdAsync(requestId);
    }
    
    public async Task<IEnumerable<Report>> Handle(GetAllReportsQuery query) =>
        await _repository.ListAsync();
    
}