using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;
namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;

public record CreateRequestCommand(
    string RequestId,
    string ClientId,
    string TechnicianId,
    string PropertyId,
    string ServiceId,
    string Status,
    DateOnly ScheduledDate,
    string ProblemDescription,
    ElectricBill Bill
);