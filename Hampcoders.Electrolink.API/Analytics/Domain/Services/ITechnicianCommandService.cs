using Hampcoders.Electrolink.API.Analytics.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Services;

public interface ITechnicianCommandService
{
    Task<Technician?> Handle(CreateTechnicianCommand command);
    
}