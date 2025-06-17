using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;

public class TechnicianInventoryCommandService(
    ITechnicianInventoryRepository inventoryRepository, 
    IComponentRepository componentRepository, // Necesario para validaciones
    IUnitOfWork unitOfWork) 
    : ITechnicianInventoryCommandService
{
    public async Task<TechnicianInventory?> Handle(CreateTechnicianInventoryCommand command)
    {
        var technicianId = new TechnicianId(command.TechnicianId);
        var existingInventory = await inventoryRepository.FindByTechnicianIdAsync(technicianId);
        if (existingInventory is not null)
            throw new InvalidOperationException("An inventory for this technician already exists.");

        var inventory = new TechnicianInventory(command);
        await inventoryRepository.AddAsync(inventory);
        await unitOfWork.CompleteAsync();
        return inventory;
    }

    public async Task<TechnicianInventory?> Handle(AddStockToInventoryCommand command)
    {
        var component = await componentRepository.FindByIdAsync(new ComponentId(command.ComponentId));
        if (component is null)
            throw new ArgumentException($"Component with id {command.ComponentId} not found.");

        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        inventory.Handle(command);
        await unitOfWork.CompleteAsync();
        return inventory;
    }

    public Task<TechnicianInventory?> Handle(UpdateAlertThresholdCommand command)
    {
        throw new NotImplementedException();
    }
    
    public Task<TechnicianInventory?> Handle(IncreaseStockCommand command)
    {
        throw new NotImplementedException();
    }

    public async Task<TechnicianInventory?> Handle(DecreaseStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        inventory.Handle(command);
        await unitOfWork.CompleteAsync();
        return inventory;
    }
}