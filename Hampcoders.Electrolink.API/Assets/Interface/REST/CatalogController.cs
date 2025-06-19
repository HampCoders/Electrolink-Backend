using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController(
    IComponentTypeCommandService componentTypeCommandService, 
    IComponentCommandService componentCommandService) : ControllerBase
{
    // --- Endpoints para ComponentType ---
    [HttpPost("types")]
    public async Task<IActionResult> CreateComponentType([FromBody] CreateComponentTypeResource resource)
    {
        var createComponentTypeCommand = CreateComponentTypeCommandFromResourceAssembler.ToCommandFromResource(resource);
        var componentType = await componentTypeCommandService.Handle(createComponentTypeCommand);
        if (componentType is null) return BadRequest();

        var resourceResponse = ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity(componentType);
        return CreatedAtAction(nameof(GetCtById), new { typeId = resourceResponse.Id }, resourceResponse);
    }

    [HttpGet("types/{typeId:int}", Name = nameof(GetCtById))] // Añadimos un nombre para la ruta
    public async Task<IActionResult> GetCtById(int typeId, [FromServices] IComponentTypeQueryService queryService)
    {
        var query = new GetComponentTypeByIdQuery(typeId);
        var componentType = await queryService.Handle(query);
        if (componentType is null) return NotFound();

        var resource = ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity(componentType);
        return Ok(resource);
    }
    // --- Endpoints para Component ---
    [HttpPost("components")]
    public async Task<IActionResult> CreateComponent([FromBody] CreateComponentResource resource)
    {
        var createComponentCommand = CreateComponentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var component = await componentCommandService.Handle(createComponentCommand);
        if (component is null) return BadRequest();

        // --- INICIO DE LA CORRECCIÓN ---

        // 1. Se llama al Assembler para convertir la entidad en un recurso para la API.
        var componentResource = ComponentResourceFromEntityAssembler.ToResourceFromEntity(component);

        // 2. Se devuelve el recurso (DTO), no la entidad del dominio.
        return Ok(componentResource); 

        // --- FIN DE LA CORRECCIÓN ---
    }
}