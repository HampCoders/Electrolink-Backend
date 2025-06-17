using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public partial class TechnicianInventory
{
    public TechnicianId TechnicianId { get; private set; }
    public TechnicianInventory(CreateTechnicianInventoryCommand command) : this()
    {
        TechnicianId = new TechnicianId(command.TechnicianId);
    }
}