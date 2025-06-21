namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;

public record CreateScheduleResource(
    string TechnicianId,
    string Day,
    string StartTime,
    string EndTime
);