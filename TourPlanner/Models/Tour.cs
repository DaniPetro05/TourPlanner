namespace TourPlanner.Models;

public class Tour
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public string TransportType { get; set; } = "";
    public double Distance { get; set; }
    public TimeSpan EstimatedTime { get; set; }
    public double popularity { get; set; }
    public double childFriendliness { get; set; }
    public string? ImagePath { get; set; }
    public string? Stops { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public string? RouteGeometry { get; set; }
    public ICollection<TourLog> TourLogs { get; set; } = new List<TourLog>();
}