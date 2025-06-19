using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;

public class ComponentStock
{
    // Identidad local (dentro del inventario)
    public Guid Id { get; private set; }

    public TechnicianId TechnicianInventoryId { get; private set; }
    public ComponentId ComponentId { get; private set; }
    
    // Propiedades mutables (pueden cambiar)
    public int QuantityAvailable { get; private set; }
    public int AlertThreshold { get; private set; }
    public DateTime LastUpdated { get; private set; }

    // Constructor 'internal' para que SOLO el agregado TechnicianInventory pueda crearlo.
    internal ComponentStock(ComponentId componentId, int quantity, int alertThreshold)
    {
        Id = Guid.NewGuid();
        ComponentId = componentId;
        QuantityAvailable = quantity;
        AlertThreshold = alertThreshold;
        LastUpdated = DateTime.UtcNow;
    }

    // Métodos 'internal' para que SOLO el agregado TechnicianInventory pueda llamarlos.
    internal void IncreaseQuantity(int amount)
    {
        QuantityAvailable += amount;
        LastUpdated = DateTime.UtcNow;
    }

    internal void DecreaseQuantity(int amount)
    {
        if (QuantityAvailable < amount)
        {
            throw new InvalidOperationException($"Not enough stock for component {ComponentId}.");
        }
        QuantityAvailable -= amount;
        LastUpdated = DateTime.UtcNow;
    }
    
    // Constructor privado para uso del ORM.
    private ComponentStock() {
        // Inicialización para evitar nulos
        TechnicianInventoryId = new TechnicianId(Guid.Empty);
        ComponentId = new ComponentId(Guid.Empty);
    }
}