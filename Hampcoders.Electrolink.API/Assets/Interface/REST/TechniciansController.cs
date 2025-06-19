using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class TechniciansController(
    ITechnicianInventoryCommandService inventoryCommandService,
    ITechnicianInventoryQueryService inventoryQueryService,
    IComponentQueryService componentQueryService) : ControllerBase // Necesitamos info de componentes
{
    
    [HttpPost("{technicianId:guid}/inventory")]
    public async Task<IActionResult> CreateTechnicianInventory(Guid technicianId)
    {
        var command = new CreateTechnicianInventoryCommand(technicianId);
        var inventory = await inventoryCommandService.Handle(command);
        if (inventory is null) return BadRequest("Could not create inventory.");

        return StatusCode(201); 
    }
    
    // Endpoint para añadir un item al stock de un técnico específico
    [HttpPost("{technicianId:guid}/inventory/stock-items")]
    public async Task<IActionResult> AddStockItemToInventory(Guid technicianId, [FromBody] AddStockToInventoryResource resource)
    {
        var command = AddStockToInventoryCommandFromResourceAssembler.ToCommandFromResource(resource, technicianId);
        var inventory = await inventoryCommandService.Handle(command);
        if (inventory is null) return BadRequest();

        // Aquí llamarías al GET para devolver el estado actualizado del inventario
        return Ok(); // Simplificado
    }

    // Endpoint para obtener el inventario completo de un técnico
    [HttpGet("{technicianId:guid}/inventory")]
    public async Task<IActionResult> GetInventoryByTechnicianId(Guid technicianId)
    {
        var query = new GetInventoryByTechnicianIdQuery(technicianId);
        var inventory = await inventoryQueryService.Handle(query);
        if (inventory is null) return NotFound();

        // Lógica de orquestación para enriquecer el DTO
        var componentIds = inventory.StockItems.Select(item => item.ComponentId.Id).ToList();
        if (!componentIds.Any())
        {
            // Si no hay items, devolvemos un inventario vacío.
            var emptyResource = TechnicianInventoryResourceFromEntityAssembler.ToResourceFromEntity(inventory, new Dictionary<Guid, string>());
            return Ok(emptyResource);
        }

        // CORREGIDO: Usamos el query y el manejador correctos.
        var components = await componentQueryService.Handle(new GetComponentsByIdsQuery(componentIds));

        var componentNames = components.ToDictionary(c => c.Id.Id, c => c.Name);

        var inventoryResource = TechnicianInventoryResourceFromEntityAssembler.ToResourceFromEntity(inventory, componentNames);
        return Ok(inventoryResource);
    }
}