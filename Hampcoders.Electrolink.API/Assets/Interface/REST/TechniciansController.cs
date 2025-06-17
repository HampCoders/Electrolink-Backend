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
        // 1. Obtenemos los IDs de los componentes en el inventario
        var componentIds = inventory.StockItems.Select(item => item.ComponentId.Id).ToList();

        // 2. Buscamos los detalles de esos componentes
        var components = await componentQueryService.Handle(new GetComponentsByIdsQuery(componentIds));
        var componentNames = components.ToDictionary(c => c.Id.Id, c => c.Name);
        // 3. Usamos el Assembler con la información extra
        var inventoryResource = TechnicianInventoryResourceFromEntityAssembler.ToResourceFromEntity(inventory, componentNames);
        return Ok(inventoryResource);
    }
}