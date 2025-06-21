using System.Net.Mime;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using Hampcoders.Electrolink.API.Analytics.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Analytics.Interface.REST;


[ApiController]
[Route("api/v1/technicians/{technicianId:int}/works")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Technicians")]

public class TechnicianWorkController 
    (IWorkQueryService workQueryService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all tutorials) by category ID",
        Description = "Retrieves all tutorials associated with a specific category ID.",
        OperationId = "GetTutorialsByCategoryId")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of tutorials for the specified category ID.")]
    public async Task<IActionResult> GetWorksByCategoryId(int technicianId)
    {
        var getAllWorksByTechnicianIdQuery = new GetAllWorksByTechniciansIdQuery(technicianId);
        var works = await workQueryService.Handle(getAllWorksByTechnicianIdQuery);
        var resources = works.Select(WorkResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}