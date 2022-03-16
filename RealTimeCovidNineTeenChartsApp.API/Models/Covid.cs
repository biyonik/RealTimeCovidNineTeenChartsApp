namespace RealTimeCovidNineTeenChartsApp.API.Models;

public class Covid
{
    public Guid Id { get; set; }
    public City City { get; set; }
    public int Count { get; set; }
    public DateTime CovidDate { get; set; }
    
}