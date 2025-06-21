namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Commands;

public record CreateWorkCommand(string Title, string Description, int TechnicianId);