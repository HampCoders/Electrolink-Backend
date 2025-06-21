using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Queries;

namespace Hamcoders.Electrolink.API.Monitoring.Domain.Services;

public interface IReportQueryService
{
    Task<Report?> GetByRequestIdAsync(Guid requestId);
    
    Task<IEnumerable<Report>> Handle(GetAllReportsQuery query);
}

