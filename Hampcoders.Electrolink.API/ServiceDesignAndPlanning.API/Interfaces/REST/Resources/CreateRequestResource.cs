using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;
public record CreateRequestResource(
    string ClientId,
    string TechnicianId,
    string PropertyId,
    string ServiceId,
    string ProblemDescription,
    DateOnly ScheduledDate,
    ElectricBill Bill,
    List<RequestPhotoResource> Photos
);