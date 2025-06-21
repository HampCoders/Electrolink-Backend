namespace Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record UpdateServiceStatusResource(Guid RequestId, string NewStatus);