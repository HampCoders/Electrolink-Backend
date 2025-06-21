using System.Net.Mime;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using Hampcoders.Electrolink.API.Analytics.Interface.REST.Resources;
using Hampcoders.Electrolink.API.Analytics.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Analytics.Interface.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Tutorial Endpoints")]


public class WorksController (IWorkCommandService workCommandService,
    IWorkQueryService workQueryService) : ControllerBase
{
    
    [HttpGet("{workId:int}")]
    [SwaggerOperation( 
        Summary = "Get Work by Id",
        Description = "Returns a tutorial by its unique identifier.",
        OperationId = "GetWorkById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Work found", typeof(WorkResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Work not found")]
    public async Task<IActionResult> GetWorkById([FromRoute] int workId)
    {
        var getWorkByIdQuery = new GetWorkByIdQuery(workId);
        var work = await workQueryService.Handle(getWorkByIdQuery);
        if (work is null) return NotFound();
        var resource = WorkResourceFromEntityAssembler.ToResourceFromEntity(work);
        return Ok(resource);
    }

    [HttpPost]
    [SwaggerOperation( 
        Summary = "Create a new Work",
        Description = "Creates a new work in the system.",
        OperationId = "CreateWork")]
    [SwaggerResponse(StatusCodes.Status201Created, "Work created", typeof(WorkResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Work to create tutorial")]
    public async Task<IActionResult> CreateWork([FromBody] CreateWorkResource resource)
    {
        var createWorkCommand = CreateWorkCommandFromResourceAssembler.ToCommandFromResource(resource);
        var work = await workCommandService.Handle(createWorkCommand);
        if (work is null) return BadRequest("Failed to create tutorial.");
        var createdResource = WorkResourceFromEntityAssembler.ToResourceFromEntity(work);
        return CreatedAtAction(nameof(GetWorkById), new { workId = createdResource.Id }, createdResource);
    }

    [HttpGet]
    [SwaggerOperation( 
        Summary = "Get All Works",
        Description = "Returns a list of all works.",
        OperationId = "GetAllWorks")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of tutorials", typeof(IEnumerable<WorkResource>))]
    public async Task<IActionResult> GetAllTutorials()
    {
        var getAllWorksQuery = new GetAllWorksQuery(); //CREAMOS UN OBJETO QUERY PARA EL USO DEL HANDLER QUE SOLO ACEPTA UN TIPO DE QUERY
        var works = await workQueryService.Handle(getAllWorksQuery);
        var resources = works.Select(WorkResourceFromEntityAssembler.ToResourceFromEntity).ToList();
        return Ok(resources);
    }

    [HttpPost("{workId:int}/Image")]
    [SwaggerOperation( 
        Summary = "Add Image to a Work",
        Description = "Adds a image asset to an existing Work.",    
        OperationId = "AddImageToWork")]
    [SwaggerResponse(StatusCodes.Status201Created, "Video added to tutorial", typeof(WorkResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to add video to tutorial")]
    public async Task<IActionResult> AddWorkToTutorial([FromBody] AddImageAssetToWorkResource resource,
        [FromRoute] int workId)
    {
        var addImageAssetToWorkCommand = AddImageAssetToWorkCommandFromResourceAssembler
            .ToCommandFromResource(resource, workId);
        var work = await workCommandService.Handle(addImageAssetToWorkCommand);
        if (work is null) return BadRequest("Failed to add video to tutorial.");
        var updatedResource = WorkResourceFromEntityAssembler.ToResourceFromEntity(work);
        return CreatedAtAction(nameof(GetWorkById), new { workId = updatedResource.Id }, updatedResource);
    }
    
    
    
}