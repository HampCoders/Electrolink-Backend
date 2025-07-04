using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST;

/// <summary>
/// Controller for managing component types and components catalog.
/// </summary>
/// <param name="componentTypeCommandService">
/// Command service for component types.
/// </param>
/// <param name="componentCommandService">
/// Command service for components.
/// </param>
[ApiController]
[Route("api/v1/[controller]")]
[SwaggerTag("Component Endpoints")]
public class ComponentsController(
    IComponentTypeCommandService componentTypeCommandService, 
    IComponentCommandService componentCommandService) : ControllerBase
{
    /// <summary>
    /// Creates a new component type.
    /// </summary>
    /// <param name="resource">The resource containing the details of the component type to create.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with the created component type resource and 201 status, or 400 if creation failed.
    /// </returns>
    [HttpPost("types")]
    [SwaggerOperation(
        Summary = "Create Component Type",
        Description = "Creates a new component type.",
        OperationId = "CreateComponentType"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Component type created", typeof(ComponentTypeResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Component type could not be created")]
    public async Task<IActionResult> CreateComponentType([FromBody] CreateComponentTypeResource resource)
    {
        var createComponentTypeCommand = CreateComponentTypeCommandFromResourceAssembler.ToCommandFromResource(resource);
        var componentType = await componentTypeCommandService.Handle(createComponentTypeCommand);
        if (componentType is null) return BadRequest();

        var resourceResponse = ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity(componentType);
        return CreatedAtAction(nameof(GetCtById), new { typeId = resourceResponse.Id }, resourceResponse);
    }

    /// <summary>
    /// Gets a component type by its unique identifier.
    /// </summary>
    /// <param name="typeId">The unique identifier of the component type.</param>
    /// <param name="queryService">The query service for component types.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with the component type resource if found, or 404 if not found.
    /// </returns>
    [HttpGet("types/{typeId:int}", Name = nameof(GetCtById))]
    [SwaggerOperation(
        Summary = "Get Component Type by Id",
        Description = "Retrieves a component type by its unique identifier.",
        OperationId = "GetComponentTypeById"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Component type found", typeof(ComponentTypeResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component type not found")]
    public async Task<IActionResult> GetCtById(int typeId, [FromServices] IComponentTypeQueryService queryService)
    {
        var query = new GetComponentTypeByIdQuery(typeId);
        var componentType = await queryService.Handle(query);
        if (componentType is null) return NotFound();

        var resource = ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity(componentType);
        return Ok(resource);
    }

    /// <summary>
    /// Creates a new component.
    /// </summary>
    /// <param name="resource">The resource containing the details of the component to create.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with the created component resource and 200 status, or 400 if creation failed.
    /// </returns>
    [HttpPost("components")]
    [SwaggerOperation(
        Summary = "Create Component",
        Description = "Creates a new component.",
        OperationId = "CreateComponent"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Component created", typeof(ComponentResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Component could not be created")]
    public async Task<IActionResult> CreateComponent([FromBody] CreateComponentResource resource)
    {
        var createComponentCommand = CreateComponentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var component = await componentCommandService.Handle(createComponentCommand);
        if (component is null) return BadRequest();


        var componentResource = ComponentResourceFromEntityAssembler.ToResourceFromEntity(component);

        return Ok(componentResource); 
    }
    
    /// <summary>
    /// Obtiene una lista de todos los tipos de componentes.
    /// </summary>
    /// <param name="queryService">El servicio de consulta para tipos de componentes.</param>
    /// <returns>
    /// <see cref="IActionResult"/> con una lista de recursos de tipos de componentes.
    /// </returns>
    [HttpGet("types")]
    [SwaggerOperation(
        Summary = "Obtener todos los tipos de componentes",
        Description = "Recupera una lista de todos los tipos de componentes disponibles.",
        OperationId = "GetAllComponentTypes"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista de tipos de componentes recuperada", typeof(IEnumerable<ComponentTypeResource>))]
    public async Task<IActionResult> GetAllComponentTypes([FromServices] IComponentTypeQueryService queryService)
    {
        var getAllComponentTypesQuery = new GetAllComponentTypesQuery();
        var componentTypes = await queryService.Handle(getAllComponentTypesQuery);

        var resources = componentTypes.Select(ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }
    
    /// <summary>
    /// Updates an existing component type.
    /// </summary>
    /// <param name="typeId">The unique identifier of the component type.</param>
    /// <param name="resource">The resource containing the updated details.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with the updated component type resource, or 404 if not found.
    /// </returns>
    [HttpPut("types/{typeId:int}")]
    [SwaggerOperation(
        Summary = "Update Component Type",
        Description = "Updates an existing component type.",
        OperationId = "UpdateComponentType"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Component type updated", typeof(ComponentTypeResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component type not found")]
    public async Task<IActionResult> UpdateComponentType(int typeId, [FromBody] UpdateComponentTypeResource resource)
    {
        var updateCommand = new UpdateComponentTypeCommand(typeId, resource.Name, resource.Description);
    
        var componentType = await componentTypeCommandService.Handle(updateCommand);
        if (componentType is null) 
            return NotFound("Component Type not found");
    
        var responseResource = ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity(componentType);
        return Ok(responseResource);
    }
    
    /// <summary>
    /// Deletes a component type by its unique identifier.
    /// </summary>
    /// <param name="typeId">The unique identifier of the component type.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with 204 status if deleted, or 404 if not found.
    /// </returns>
    [HttpDelete("types/{typeId:int}")]
    [SwaggerOperation(
        Summary = "Delete Component Type",
        Description = "Deletes a component type by its unique identifier.",
        OperationId = "DeleteComponentType"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Component type deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component type not found")]
    public async Task<IActionResult> DeleteComponentType(int typeId)
    {
        var deleteCommand = new DeleteComponentTypeCommand(typeId);
    
        var result = await componentTypeCommandService.Handle(deleteCommand);
    
        if (!result) 
            return NotFound("Component Type not found");
        
        return NoContent();
    }
    
    /// <summary>
    /// Deletes a component by its unique identifier.
    /// </summary>
    /// <param name="componentId">The unique identifier of the component.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with 204 status if deleted, or 404 if not found.
    /// </returns>
    [HttpDelete("components/{componentId:guid}")]
    [SwaggerOperation(
        Summary = "Delete Component",
        Description = "Deletes a component by its unique identifier.",
        OperationId = "DeleteComponent"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Component deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component not found")]
    public async Task<IActionResult> DeleteComponent(Guid componentId)
    {
        var deleteCommand = new DeleteComponentCommand(componentId);

        var result = await componentCommandService.Handle(deleteCommand);

        if (!result)
            return NotFound("Component not found");

        return NoContent();
    }
    
    /// <summary>
    /// Updates an existing component.
    /// </summary>
    /// <param name="componentId">The unique identifier of the component.</param>
    /// <param name="resource">The resource containing the updated details.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with the updated component resource, or 404 if not found.
    /// </returns>
    [HttpPut("components/{componentId:guid}")]
    [SwaggerOperation(
        Summary = "Update Component",
        Description = "Updates an existing component.",
        OperationId = "UpdateComponent"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Component updated", typeof(ComponentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component not found")]
    public async Task<IActionResult> UpdateComponent(Guid componentId, [FromBody] UpdateComponentResource resource)
    {
        var updateCommand = UpdateComponentCommandFromResourceAssembler.ToCommandFromResource(resource,componentId);

        var component = await componentCommandService.Handle(updateCommand);
        if (component is null)
            return NotFound("Component not found");

        var responseResource = ComponentResourceFromEntityAssembler.ToResourceFromEntity(component);
        return Ok(responseResource);
    }
    
    /// <summary>
    /// Gets a list of all components.
    /// </summary>
    /// <param name="queryService">The query service for components.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with a list of component resources.
    /// </returns>
    [HttpGet("components")] // <--- ¡EL ENDPOINT QUE FALTABA!
    [SwaggerOperation(
        Summary = "Get All Components",
        Description = "Retrieves a list of all components available.",
        OperationId = "GetAllComponents"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of components retrieved", typeof(IEnumerable<ComponentResource>))]
    public async Task<IActionResult> GetAllComponents([FromServices] IComponentQueryService queryService)
    {
        // 1. Crea y maneja la query para obtener todos los componentes.
        var getAllComponentsQuery = new GetAllComponentsQuery();
        var components = await queryService.Handle(getAllComponentsQuery);

        // 2. Transforma las entidades del dominio a recursos de la API.
        var resources = components.Select(ComponentResourceFromEntityAssembler.ToResourceFromEntity);
        
        // 3. Devuelve la lista de recursos con un estado 200 OK.
        return Ok(resources);
    }
    
}