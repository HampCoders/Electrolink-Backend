namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateComponentInfoCommand(Guid Id, string Name, string Description);
