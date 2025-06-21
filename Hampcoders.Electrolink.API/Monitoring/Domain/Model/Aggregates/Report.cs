using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;

public class Report
{
    public Guid ReportId { get; private set; }
    public Guid RequestId { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    
    private readonly List<ReportPhoto> _photos = new();
    public IReadOnlyCollection<ReportPhoto> Photos => _photos;
    public Report(Guid reportId, Guid requestId, string description, DateTime date)
    {
        ReportId = reportId;
        RequestId = requestId;
        Description = description;
        Date = date;
    }

    public Report(Guid requestId, string description)
        : this(Guid.NewGuid(), requestId, description, DateTime.UtcNow)
    {
    }

    public void AddPhoto(string url, string type)
    {
        if (_photos.Any(p => p.Url == url && p.Type == type)) return;
        _photos.Add(new ReportPhoto(Guid.NewGuid(), ReportId, url, type));
    }
   
}