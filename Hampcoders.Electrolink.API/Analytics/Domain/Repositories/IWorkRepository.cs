using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Repositories;

public interface IWorkRepository : IBaseRepository<Work>
{
    Task<IEnumerable<Work>> FindByTechnicianIdAsync(int technicianId);
    
    Task<bool> ExistsByTitleAsync(string title);
}