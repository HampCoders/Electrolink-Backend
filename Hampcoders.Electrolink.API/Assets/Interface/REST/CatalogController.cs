using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
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
    [HttpPost("types")]
    public async Task<IActionResult> CreateComponentType([FromBody] CreateComponentTypeResource resource)
    {
        var createComponentTypeCommand = CreateComponentTypeCommandFromResourceAssembler.ToCommandFromResource(resource);
        var componentType = await componentTypeCommandService.Handle(createComponentTypeCommand);
        if (componentType is null) return BadRequest();

        var resourceResponse = ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity(componentType);
        return CreatedAtAction(nameof(GetCtById), new { typeId = resourceResponse.Id }, resourceResponse);
    }

    [HttpGet("types/{typeId:int}", Name = nameof(GetCtById))]
    public async Task<IActionResult> GetCtById(int typeId, [FromServices] IComponentTypeQueryService queryService)
    {
        var query = new GetComponentTypeByIdQuery(typeId);
        var componentType = await queryService.Handle(query);
        if (componentType is null) return NotFound();

        var resource = ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity(componentType);
        return Ok(resource);
    }
    [HttpPost("components")]
    public async Task<IActionResult> CreateComponent([FromBody] CreateComponentResource resource)
    {
        var createComponentCommand = CreateComponentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var component = await componentCommandService.Handle(createComponentCommand);
        if (component is null) return BadRequest();


        var componentResource = ComponentResourceFromEntityAssembler.ToResourceFromEntity(component);

        return Ok(componentResource); 
    }
    
    [HttpPut("types/{typeId:int}")]
    public async Task<IActionResult> UpdateComponentType(int typeId, [FromBody] UpdateComponentTypeResource resource)
    {
        var updateCommand = new UpdateComponentTypeCommand(typeId, resource.Name, resource.Description);
    
        var componentType = await componentTypeCommandService.Handle(updateCommand);
        if (componentType is null) 
            return NotFound("Component Type not found");
    
        var responseResource = ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity(componentType);
        return Ok(responseResource);
    }
    
    [HttpDelete("types/{typeId:int}")]
    public async Task<IActionResult> DeleteComponentType(int typeId)
    {
        var deleteCommand = new DeleteComponentTypeCommand(typeId);
    
        var result = await componentTypeCommandService.Handle(deleteCommand);
    
        if (!result) 
            return NotFound("Component Type not found");
        
        return NoContent();
    }
    
    [HttpDelete("components/{componentId:guid}")]
    public async Task<IActionResult> DeleteComponent(Guid componentId)
    {
        var deleteCommand = new DeleteComponentCommand(componentId);

        var result = await componentCommandService.Handle(deleteCommand);

        if (!result)
            return NotFound("Component not found");

        return NoContent();
    }
    
    [HttpPut("components/{componentId:guid}")]
    public async Task<IActionResult> UpdateComponent(Guid componentId, [FromBody] UpdateComponentResource resource)
    {
        var updateCommand = UpdateComponentCommandFromResourceAssembler.ToCommandFromResource(resource,componentId);

        var component = await componentCommandService.Handle(updateCommand);
        if (component is null)
            return NotFound("Component not found");

        var responseResource = ComponentResourceFromEntityAssembler.ToResourceFromEntity(component);
        return Ok(responseResource);
    }
    
}