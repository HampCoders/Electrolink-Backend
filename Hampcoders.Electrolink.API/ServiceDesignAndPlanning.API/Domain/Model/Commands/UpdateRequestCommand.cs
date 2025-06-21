namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;

using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;

public record UpdateRequestCommand(
    string RequestId,
    string ClientId,
    string TechnicianId,
    string PropertyId,
    string ServiceId,
    DateOnly ScheduledDate,
    string ProblemDescription,
    ElectricBill Bill,
    List<RequestPhotoResource> Photos
);