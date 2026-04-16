namespace TourPlanner;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}

public class Tour
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Location { get; set; }
    
}

public class User {
    public int id { get; set; }
    public string? username { get; set; }
    public string? password { get; set; }
    public string? email { get; set; }
}