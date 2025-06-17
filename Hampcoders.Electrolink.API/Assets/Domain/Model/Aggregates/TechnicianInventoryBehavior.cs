using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public partial class TechnicianInventory
{
    private readonly List<ComponentStock> _stockItems;
    public IReadOnlyCollection<ComponentStock> StockItems => _stockItems.AsReadOnly();
    
    public TechnicianInventory()
    {
        _stockItems = new List<ComponentStock>();
    }

    private ComponentStock? GetStockItemByComponentId(ComponentId componentId)
    {
        return _stockItems.FirstOrDefault(s => s.ComponentId == componentId);
    }
    
    public void Handle(AddStockToInventoryCommand command)
    {
        if (TechnicianId.Id != command.TechnicianId) return;

        var componentId = new ComponentId(command.ComponentId);
        if (GetStockItemByComponentId(componentId) != null)
        {
            throw new InvalidOperationException($"Stock for component {componentId} already exists in this inventory.");
        }
        
        var newStockItem = new ComponentStock(componentId, command.Quantity, command.AlertThreshold);
        _stockItems.Add(newStockItem);
    }

    public void Handle(DecreaseStockCommand command)
    {
        if (TechnicianId.Id != command.TechnicianId) return;
        
        var componentId = new ComponentId(command.ComponentId);
        var stockItem = GetStockItemByComponentId(componentId) ?? throw new InvalidOperationException("Stock item not found.");
        
        // El padre (agregado) le ordena al hijo (entidad) que se modifique.
        stockItem.DecreaseQuantity(command.AmountToDecrease);
    }
}