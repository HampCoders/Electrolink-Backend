namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record PropertyResource(
    Guid Id,
    string FullAddress,
    string Region,
    string District
    //string Status
    //string? PhotoUrl 
);