using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Profiles.Infrastructure.Persistence.EFC.Repositories;

public class ProfileRepository(AppDbContext context)
  : BaseRepository<Profile>(context), IProfileRepository
{
  public async Task<Profile?> FindByEmailAsync(string email)
  {
    return await Context.Set<Profile>()
      .Include(p => p.HomeOwner)
      .Include(p => p.Technician)
      .FirstOrDefaultAsync(p => p.Email.Address == email);
  }

  public async Task<IEnumerable<Profile>> FindByRoleAsync(Role role)
  {
    return await Context.Set<Profile>()
      .Include(p => p.HomeOwner)
      .Include(p => p.Technician)
      .Where(p => p.Role == role)
      .ToListAsync();
  }
}
