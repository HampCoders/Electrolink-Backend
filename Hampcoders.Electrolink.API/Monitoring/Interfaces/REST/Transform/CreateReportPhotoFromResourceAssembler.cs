using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class CreateReportPhotoCommandFromResourceAssembler
{
    public static AddReportPhotoCommand ToCommandFromResource(CreateReportPhotoResource resource)
        => new(resource.ReportId, resource.Url, resource.Type);
}