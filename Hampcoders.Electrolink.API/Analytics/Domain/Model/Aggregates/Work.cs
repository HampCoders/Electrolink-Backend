namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;

public partial class Work
{
    public int Id { get;}
    public string Tittle { get; private set; }
    public string Description { get; private set; }
    public Technician Technician { get; internal set; }
    public int TechnicianId { get; private set; }

    public Work(string tittle, string description, int technicianId) : this()
    {
        Tittle = tittle;
        Description = description;
        TechnicianId = technicianId;
    }
    
    
}
