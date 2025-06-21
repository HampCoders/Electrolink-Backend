using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.CommandServices;

public class WorkCommandService(
    IWorkRepository workRepository,
    IUnitOfWork unitOfWork,
    ITechnicianRepository technicianRepository) : IWorkCommandService
{
    public async Task<Work?> Handle(AddImageAssetToWorkCommand command)
    {
        var work = await workRepository.FindByIdAsync(command.WorkId);
        if(work is null) throw new ArgumentException($"Work with ID {command.WorkId} not found.");
        work.AddImage(command.ImageUrl);
        await unitOfWork.CompleteAsync();
        return work;
    }
    public async Task<Work?> Handle(CreateWorkCommand command)
    {
        var technician = await technicianRepository.FindByIdAsync(command.TechnicianId);
        if (technician is null) throw new ArgumentException($"Category with ID {command.TechnicianId} not found.");

        var tutorial = new Work(command);
        await workRepository.AddAsync(tutorial);
        await unitOfWork.CompleteAsync();
        tutorial.Technician= technician;
        return tutorial;
    }
}
