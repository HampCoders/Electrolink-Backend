using Hampcoders.Electrolink.API.Analytics.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;

public class Technician
{
    public Technician()
    {
        Name = string.Empty;
    }

    public Technician(string name, string email)
    {
        Name = name;
        Email = email;
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    public Technician(CreateTechnicianCommand command): this(command.Name, command.Email){}
}