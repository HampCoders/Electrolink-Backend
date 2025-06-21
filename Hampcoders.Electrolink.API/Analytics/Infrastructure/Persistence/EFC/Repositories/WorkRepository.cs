using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Repositories;

public class WorkRepository(AppDbContext context) : BaseRepository<Work>(context), IWorkRepository
{
    
    
    public async Task<IEnumerable<Work>> FindByTechnicianIdAsync(int technicianId)
    {
        return await Context.Set<Work>()
            .Include(work => work.Technician)
            .Where(work => work.TechnicianId == technicianId)
            .ToListAsync();
    }

    public async Task<bool> ExistsByTitleAsync(string title)
    {
        return await Context.Set<Work>()
            .AnyAsync(work => work.Title == title);
    }

    public new async Task<Work?> FindByIdAsync(int id)
    {
        return await Context.Set<Work>()
            .Include(work => work.Technician)
            .FirstOrDefaultAsync(tutorial => tutorial.Id == id);
    }
    
    public new async Task<IEnumerable<Work>> ListAsync()
    {
        return await Context.Set<Work>()
            .Include(work => work.Technician)
            .ToListAsync();
    }
}