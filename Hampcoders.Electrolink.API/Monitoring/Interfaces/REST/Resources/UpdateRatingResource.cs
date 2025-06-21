namespace Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
public record UpdateRatingResource(Guid RatingId, int Score, string Comment);