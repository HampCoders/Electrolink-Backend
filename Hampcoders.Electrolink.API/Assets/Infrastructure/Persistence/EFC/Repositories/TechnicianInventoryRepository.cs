using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

public class TechnicianInventoryRepository(AppDbContext context) : BaseRepository<TechnicianInventory>(context), ITechnicianInventoryRepository
{
    // El método principal para buscar un inventario
    public async Task<TechnicianInventory?> FindByTechnicianIdAsync(TechnicianId technicianId)
    {
        // Es CRUCIAL incluir las entidades hijas (StockItems) al cargar el agregado
        return await Context.Set<TechnicianInventory>()
            .Include(i => i.StockItems)
            .FirstOrDefaultAsync(i => i.TechnicianId == technicianId);
    }
    
    public Task AddComponentStockAsync(ComponentStock stockItem)
    {
        Context.Set<ComponentStock>().Add(stockItem);
        return Task.CompletedTask;
    }
}