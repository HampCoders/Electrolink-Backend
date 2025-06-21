using Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Repositories;

public class TechnicianRepository(AppDbContext context) :
    BaseRepository<Technician>(context),
    ITechnicianRepository;