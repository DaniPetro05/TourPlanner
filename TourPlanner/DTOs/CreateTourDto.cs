namespace TourPlanner.DTOs;

public class CreateTourDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public string TransportType { get; set; } = "driving-car";
    public List<string>? Stops { get; set; }
}