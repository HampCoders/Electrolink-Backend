using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST;

[ApiController]
[Route("api/v1/owners/{ownerId:guid}/properties")]
public class PropertiesController(IPropertyCommandService propertyCommandService, IPropertyQueryService propertyQueryService) : ControllerBase
{
    /// <summary>
    /// Obtiene una lista de propiedades para un propietario, con opción de filtrado.
    /// </summary>
    /// <param name="ownerId">El ID del propietario (de la URL).</param>
    /// <param name="city">Filtra por ciudad.</param>
    /// <param name="district">Filtra por distrito.</param>
    /// <param name="region">Filtra por región.</param>
    /// <param name="street">Filtra por calle.</param>
    /// <returns>Una lista de propiedades que cumplen los criterios.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllPropertiesByOwner(
        Guid ownerId, 
        [FromQuery] string? city, 
        [FromQuery] string? district, 
        [FromQuery] string? region, 
        [FromQuery] string? street)
    {
        // 1. La query ahora incluye el ownerId y todos los filtros posibles.
        var query = new GetAllPropertiesByOwnerIdQuery(ownerId, city, district, region, street);
        var properties = await propertyQueryService.Handle(query);

        // 2. Transformar y devolver.
        var resources = properties.Select(PropertyResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProperty(Guid ownerId, [FromBody] CreatePropertyResource resource)
    {
        // ✅ Solución: Pasamos el ownerId de la URL al comando.
        var createPropertyCommand = CreatePropertyCommandFromResourceAssembler.ToCommandFromResource(resource, ownerId);

        var property = await propertyCommandService.Handle(createPropertyCommand);
        if (property is null) return BadRequest();
        var propertyResource = PropertyResourceFromEntityAssembler.ToResourceFromEntity(property);
    
        // ✅ Solución: La URL de respuesta ahora es completa y correcta.
        return CreatedAtAction(nameof(GetPropertyById), new { ownerId, propertyId = propertyResource.Id }, propertyResource);
    }

    [HttpGet("{propertyId:guid}")]
    public async Task<IActionResult> GetPropertyById(Guid ownerId, Guid propertyId)
    {
        var getPropertyByIdQuery = new GetPropertyByIdQuery(propertyId, ownerId);
        var property = await propertyQueryService.Handle(getPropertyByIdQuery);
        if (property == null) return NotFound();
        
        var propertyResource = PropertyResourceFromEntityAssembler.ToResourceFromEntity(property);
        return Ok(propertyResource);
    }
    
    /// <summary>
/// Actualiza una propiedad existente.
/// </summary>
/// <param name="propertyId">El identificador único de la propiedad.</param>
/// <param name="resource">El recurso que contiene los detalles actualizados.</param>
/// <returns>
/// <see cref="IActionResult"/> con el recurso de la propiedad actualizada o 404 si no se encuentra.
/// </returns>
[HttpPut("{propertyId:guid}")]
public async Task<IActionResult> UpdateProperty(Guid propertyId, [FromBody] UpdatePropertyResource resource)
{
    var updateCommand = UpdatePropertyCommandFromResourceAssembler.ToCommandFromResource(resource, propertyId);
    
    var property = await propertyCommandService.Handle(updateCommand);
    if (property is null)
        return NotFound("Propiedad no encontrada");

    var responseResource = PropertyResourceFromEntityAssembler.ToResourceFromEntity(property);
    return Ok(responseResource);
}

/// <summary>
/// Elimina una propiedad por su identificador único.
/// </summary>
/// <param name="propertyId">El identificador único de la propiedad.</param>
/// <returns>
/// <see cref="IActionResult"/> con estado 204 si se eliminó, o 404 si no se encontró.
/// </returns>
[HttpDelete("{propertyId:guid}")]
public async Task<IActionResult> DeleteProperty(Guid propertyId)
{
    var deleteCommand = new DeletePropertyCommand(propertyId);
    
    var result = await propertyCommandService.Handle(deleteCommand);
    
    if (!result)
        return NotFound("Propiedad no encontrada");
    
    return NoContent();
}

/// <summary>
/// Agrega una foto a una propiedad existente.
/// </summary>
/// <param name="propertyId">El identificador único de la propiedad.</param>
/// <param name="resource">El recurso con la URL de la foto.</param>
/// <returns>
/// <see cref="IActionResult"/> con la propiedad actualizada o 404 si no se encuentra.
/// </returns>
[HttpPut("{propertyId:guid}/photo")]
public async Task<IActionResult> AddPhotoToProperty(Guid propertyId, [FromBody] AddPhotoResource resource)
{
    var command = new AddPhotoToPropertyCommand(propertyId, resource.PhotoUrl);
    
    var property = await propertyCommandService.Handle(command);
    if (property is null)
        return NotFound("Propiedad no encontrada");

    var responseResource = PropertyResourceFromEntityAssembler.ToResourceFromEntity(property);
    return Ok(responseResource);
}

/// <summary>
/// Actualiza la dirección de una propiedad existente.
/// </summary>
/// <param name="propertyId">El identificador único de la propiedad.</param>
/// <param name="resource">El recurso con la nueva dirección.</param>
/// <returns>
/// <see cref="IActionResult"/> con la propiedad actualizada o 404 si no se encuentra.
/// </returns>
[HttpPut("{propertyId:guid}/address")]
public async Task<IActionResult> UpdatePropertyAddress(Guid propertyId, [FromBody] UpdateAddressResource resource)
{
    var command = new UpdatePropertyAddressCommand(propertyId, resource.NewAddress);
    
    var property = await propertyCommandService.Handle(command);
    if (property is null)
        return NotFound("Propiedad no encontrada");

    var responseResource = PropertyResourceFromEntityAssembler.ToResourceFromEntity(property);
    return Ok(responseResource);
}
}