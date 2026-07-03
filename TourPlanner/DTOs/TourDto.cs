namespace TourPlanner.DTOs;

public class TourDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public string TransportType { get; set; } = "";
    public List<string> Stops { get; set; } = new();
    public double Distance { get; set; }
    public string EstimatedTime { get; set; } = "";
    public string Popularity { get; set; } = "";
    public string ChildFriendliness { get; set; } = "";
    public string? RouteGeometry { get; set; }
}