using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class TechnicianInventoryResourceFromEntityAssembler
{
    // Para este assembler, el QueryService necesitaría pasarle la información extra (ej. nombres de componentes).
    // Por simplicidad, aquí asumimos que se la pasamos en un diccionario.
    public static TechnicianInventoryResource ToResourceFromEntity(TechnicianInventory entity, Dictionary<Guid, string> componentNames)
    {
        var stockItemsResources = entity.StockItems.Select(stockItem => new ComponentStockResource(
            stockItem.ComponentId.Id,
            componentNames.GetValueOrDefault(stockItem.ComponentId.Id, "Unknown Component"),
            stockItem.QuantityAvailable,
            stockItem.AlertThreshold
        )).ToList();

        return new TechnicianInventoryResource(entity.TechnicianId.Id, stockItemsResources);
    }
}