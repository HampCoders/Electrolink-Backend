using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class PropertiesController(IPropertyCommandService propertyCommandService, IPropertyQueryService propertyQueryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyResource resource)
    {
        // 1. Convierte el Recurso (DTO de la API) en un Comando (intención del Dominio)
        var createPropertyCommand = CreatePropertyCommandFromResourceAssembler.ToCommandFromResource(resource);
        
        // 2. Envía el comando al servicio de aplicación para que lo procese
        var property = await propertyCommandService.Handle(createPropertyCommand);
        
        // 3. Si tiene éxito, convierte la entidad devuelta en un Recurso para la respuesta
        if (property is null) return BadRequest();
        var propertyResource = PropertyResourceFromEntityAssembler.ToResourceFromEntity(property);
        
        // 4. Devuelve una respuesta HTTP 201 Created con la ubicación y el objeto creado
        return CreatedAtAction(nameof(GetPropertyById), new { propertyId = propertyResource.Id }, propertyResource);
    }

    [HttpGet("{propertyId:guid}")]
    public async Task<IActionResult> GetPropertyById(Guid propertyId)
    {
        var getPropertyByIdQuery = new GetPropertyByIdQuery(propertyId);
        var property = await propertyQueryService.Handle(getPropertyByIdQuery);
        if (property == null) return NotFound();
        
        var propertyResource = PropertyResourceFromEntityAssembler.ToResourceFromEntity(property);
        return Ok(propertyResource);
    }
}