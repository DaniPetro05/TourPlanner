namespace TourPlanner.DTOs;

public class TourLogDto
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public DateTime Date { get; set; }
    public string Comment { get; set; } = "";
    public int Difficulty { get; set; }
    public double TotalDistance { get; set; }
    public double TotalTime { get; set; }
    public int Rating { get; set; }
}