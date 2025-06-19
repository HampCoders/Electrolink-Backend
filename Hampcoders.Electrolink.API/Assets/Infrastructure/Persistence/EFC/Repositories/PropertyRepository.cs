using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

public class PropertyRepository(AppDbContext context) : BaseRepository<Property>(context), IPropertyRepository
{
    public async Task<IEnumerable<Property>> FindByOwnerIdAsync(OwnerId ownerId)
    {
        return await Context.Set<Property>()
            .Include(p => p.Photo) 
            .Where(p => p.OwnerId == ownerId)
            .ToListAsync();
    }
    
    public async Task<Property?> FindByIdAsync(PropertyId id)
    {
        return await Context.Set<Property>()
            .Include(p => p.Photo)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}