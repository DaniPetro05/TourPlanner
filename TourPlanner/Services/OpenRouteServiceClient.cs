using System.Net.Http.Headers;
using System.Text.Json;
using TourPlanner.Constants;
using TourPlanner.Services;
using TourPlanner.Utilites;
using TourPlanner.Interfaces;

public class OpenRouteServiceClient : IOpenRouteServiceClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public OpenRouteServiceClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<OpenRouteResult> GetRouteAsync(string from, string to, string transportType, List<string>? stops)
    {
        var apiKey = _config["OpenRouteService:ApiKey"];

        if (!TransportTypes.All.Contains(transportType))
        {
            throw new ArgumentException("Invalid transport type.");
        }

        var url = $"https://api.openrouteservice.org/v2/directions/{transportType}";

        var start = await GeocodeAsync(from);
        var end = await GeocodeAsync(to);

        var coordinates = new List<double[]>();

        coordinates.Add(new[] { start.lon, start.lat });

        if (stops != null)
        {
            foreach (var stop in stops)
            {
                if (string.IsNullOrWhiteSpace(stop))
                    continue;

                var s = await GeocodeAsync(stop);

                coordinates.Add(new[]
                {
                    s.lon,
                    s.lat
                });
            }
        }

        coordinates.Add(new[]
        {
            end.lon,
            end.lat
        });

        var requestBody = new
        {
            coordinates,
            geometry = true,
            geometry_simplify = false,
            instructions = false,
            elevation = false,
            preference = "recommended",
            format = "geojson"
        };

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        //request.Headers.Authorization = new AuthenticationHeaderValue(apiKey);
        //request.Headers.Add("Authorization", apiKey);
        request.Headers.TryAddWithoutValidation("Authorization", apiKey);
        request.Content = new StringContent(JsonSerializer.Serialize(requestBody));
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await _http.SendAsync(request);
        //response.EnsureSuccessStatusCode();

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Status: {response.StatusCode}");
            Console.WriteLine(error);

            //throw new Exception(error);
            /*throw new Exception(
                "No valid route could be found. Please check your start, destination and stops."
            );*/

            throw new Exception(
                $"No {transportType} route could be found for the selected start, destination and stops."
            );
        }

        var json = await response.Content.ReadAsStringAsync();

        var doc = JsonDocument.Parse(json);

        var route = doc.RootElement.GetProperty("routes")[0];
        
        var summary = route.GetProperty("summary");

        var encoded = route.GetProperty("geometry").GetString();

        if (string.IsNullOrWhiteSpace(encoded))
        {
            throw new Exception("OpenRouteService returned no route geometry.");
        }

        var decoded = PolylineDecoder.Decode(encoded);

        var geoJson = new
        {
            type = "LineString",
            coordinates = decoded.Select(p => new[] { p.lon, p.lat })
        };

        var distance = summary.GetProperty("distance").GetDouble() / 1000.0;
        var duration = summary.GetProperty("duration").GetDouble() / 60.0;

        return new OpenRouteResult
        {
            Distance = distance,
            Duration = duration,
            Geometry = JsonSerializer.Serialize(geoJson)
        };
    }

    private async Task<(double lon, double lat)> GeocodeAsync(string location)
    {
        var apiKey = _config["OpenRouteService:ApiKey"];

        var url = $"https://api.openrouteservice.org/geocode/search?api_key={apiKey}&text={Uri.EscapeDataString(location)}";

        var response = await _http.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        var features = doc.RootElement.GetProperty("features");

        if (features.GetArrayLength() == 0)
        {
            throw new Exception($"Location '{location}' could not be found.");
        }

        var coords = features[0]
            .GetProperty("geometry")
            .GetProperty("coordinates");

        return (
            coords[0].GetDouble(),   // longitude
            coords[1].GetDouble()    // latitude
        );
    }
}