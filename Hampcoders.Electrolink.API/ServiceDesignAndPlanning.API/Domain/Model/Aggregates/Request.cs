using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Entities;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;

public partial class Request
{
    public string RequestId { get; private set; }
    public string ClientId { get; private set; }
    public string TechnicianId { get; private set; }
    public string PropertyId { get; private set; }
    public string ServiceId { get; private set; }
    public string Status { get; private set; }
    public DateOnly ScheduledDate { get; private set; }
    public string ProblemDescription { get; set; }

    public List<RequestPhoto> Photos { get; private set; } = new();
    public ElectricBill Bill { get; private set; }

    public Request() {
        Status = "Pending";
        ProblemDescription = string.Empty;
    }

    public Request(CreateRequestCommand command) : this()
    {
        RequestId = command.RequestId;
        ClientId = command.ClientId;
        TechnicianId = command.TechnicianId;
        PropertyId = command.PropertyId;
        ServiceId = command.ServiceId;
        Status = command.Status ?? "Pending";
        ScheduledDate = command.ScheduledDate;
        ProblemDescription = command.ProblemDescription;
        Bill = command.Bill;
    }
}