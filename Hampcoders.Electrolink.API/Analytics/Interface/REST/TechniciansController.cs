using System.Net.Mime;
using Hampcoders.Electrolink.API.Analytics.Application.Internal.QueryServices;
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
[SwaggerTag("Avaliable Category Endpoints")]


public class TechniciansController(ITechnicianCommandService technicianCommandService,
    ITechnicianQueryService technicianQueryService): ControllerBase
{
    [HttpGet("{technicianId:int}")]
    [SwaggerOperation(
        Summary = "Get a technician by ID",
        Description = "Retrieves a technician by their unique identifier.",
        OperationId = "GetTechnicianById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Category found", typeof(TechnicianResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Category not found")]
    public async Task<IActionResult> GetTechnicianById(int technicianId)
    {
        var getTechnicianByIdQuery = new GetTechnicianByIdQuery(technicianId);
        var technician = await technicianQueryService.Handle(getTechnicianByIdQuery);
        if (technician is null) return NotFound();
        var resource = TechnicianResourceFromEntityAssembler.ToResourceFromEntity(technician);
        return Ok(resource);
    }
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create technician",
        Description = "Creates a new technician in the system.",
        OperationId = "CreateTechnician")]
    [SwaggerResponse(StatusCodes.Status201Created, "technician created", typeof(TechnicianResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The technician could not be created")]   
    public async Task<IActionResult> CreateTechnician([FromBody] CreateTechnicianResource resource)
    {
        var createTechnicianCommand = CreateTechnicianCommandFromResourceAssembler.ToCommandFromResource(resource);
        var technician = await technicianCommandService.Handle(createTechnicianCommand);
        if (technician is null) return BadRequest("technician could not be created.");
        var createdResource = TechnicianResourceFromEntityAssembler.ToResourceFromEntity(technician);
        return CreatedAtAction(nameof(GetTechnicianById), new { technicianId = createdResource.Id }, createdResource);
    }
    
    
    [HttpGet]
    [SwaggerOperation( 
        Summary = "Get All Categories",
        Description = "Returns a list of all categories in the system.",
        OperationId = "GetAllCategories")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of categories", typeof(IEnumerable<TechnicianResource>))]
    public async Task<IActionResult> GetAllCategories()
    {
        var getAllTechniciansQuery = new GetAllTechniciansQuery();
        var technicians = await technicianQueryService.Handle(getAllTechniciansQuery);
        var resources = technicians.Select(TechnicianResourceFromEntityAssembler.ToResourceFromEntity).ToList();
        return Ok(resources);
    }
}