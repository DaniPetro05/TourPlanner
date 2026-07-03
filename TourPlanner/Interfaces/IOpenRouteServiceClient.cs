using TourPlanner.Services;

namespace TourPlanner.Interfaces;

public interface IOpenRouteServiceClient
{
    Task<OpenRouteResult> GetRouteAsync(string from, string to, string transportType, List<string>? stops);
}