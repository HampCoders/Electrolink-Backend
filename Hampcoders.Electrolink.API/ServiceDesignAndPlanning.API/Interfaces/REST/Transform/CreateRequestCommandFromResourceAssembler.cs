using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Transform;

public static class CreateRequestCommandFromResourceAssembler
{
    public static CreateRequestCommand ToCommandFromResource(CreateRequestResource r) =>
        new CreateRequestCommand(
            Guid.NewGuid().ToString(),
            r.ClientId,
            r.TechnicianId,
            r.PropertyId,
            r.ServiceId,
            "Pending",                  
            r.ScheduledDate,
            r.ProblemDescription,
            new ElectricBill(
                r.Bill.BillingPeriod,
                r.Bill.EnergyConsumed,
                r.Bill.AmountPaid,
                r.Bill.BillImageUrl
            )
        );
}
