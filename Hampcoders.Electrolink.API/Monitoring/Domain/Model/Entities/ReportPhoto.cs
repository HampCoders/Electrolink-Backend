using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;

public class ReportPhoto
{
    public Guid PhotoId { get; set; }
    public string Url { get; set; } = null!;
    public string Type { get; set; } = null!;
    
    public Guid ReportId { get; set; } 
    public Report Report { get; set; } = null!;
    public ReportPhoto(Guid photoId, Guid reportId, string url, string type)
    {
        PhotoId = photoId;
        ReportId = reportId;
        Url = url;
        Type = type;
    }

    protected ReportPhoto() { }
}