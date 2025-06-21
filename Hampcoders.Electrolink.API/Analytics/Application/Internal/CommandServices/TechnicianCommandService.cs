using Hampcoders.Electrolink.API.Analytics.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.CommandServices;

public class TechnicianCommandService(ITechnicianRepository technicianRepository, IUnitOfWork unitOfWork)
    : ITechnicianCommandService
{
    public async Task<Technician?> Handle(CreateTechnicianCommand command)
    {
        var technician = new Technician(command);
        await technicianRepository.AddAsync(technician);
        await unitOfWork.CompleteAsync();
        return technician;
    }
}