using Hampcoders.Electrolink.API.Analytics.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;

public partial class Work
{
    public int Id { get;}
    public string Title { get; private set; }
    public string Description { get; private set; }
    
    public Technician Technician { get; internal set; }
    public int TechnicianId { get; private set; }

    public Work(string title, string description, int technicianId) : this()
    {
        Title = title;
        Description = description;
        TechnicianId = technicianId;
    }
    
    public Work(CreateWorkCommand command) : 
        this(command.Title, command.Description, command.TechnicianId) {}
}
