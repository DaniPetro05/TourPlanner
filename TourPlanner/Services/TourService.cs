using Microsoft.EntityFrameworkCore;
using TourPlanner.Data;
using TourPlanner.DTOs;
using TourPlanner.Interfaces;
using TourPlanner.Models;
using TourPlanner.Constants;

namespace TourPlanner.Services;

public class TourService : ITourService
{
    private readonly ApplicationDbContext _context;
    private readonly IOpenRouteServiceClient _ors;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TourService(ApplicationDbContext context, IOpenRouteServiceClient ors, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _ors = ors;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<TourDto>> GetAllAsync()
    {
        var userId = GetCurrentUserId();
        
        var tours = await _context.Tours
            .Include(t => t.TourLogs)
            .Where(t => t.UserId == userId)
            .ToListAsync();

        return tours.Select(t => new TourDto
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            From = t.From,
            To = t.To,
            TransportType = t.TransportType,
            Distance = t.Distance,
            EstimatedTime = t.EstimatedTime.ToString(),
            Popularity = CalculatePopularity(t).ToString(),
            ChildFriendliness = CalculateChildFriendliness(t).ToString(),
            Stops = string.IsNullOrWhiteSpace(t.Stops)
                ? new List<string>()
                : t.Stops.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            RouteGeometry = t.RouteGeometry
        }).ToList();
    }

    public async Task<TourDto?> GetByIdAsync(int id)
    {
        var userId = GetCurrentUserId();

        var t = await _context.Tours.Include(t => t.TourLogs).FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (t == null) return null;

        return new TourDto
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            From = t.From,
            To = t.To,
            TransportType = t.TransportType,
            Distance = t.Distance,
            EstimatedTime = t.EstimatedTime.ToString(),
            Popularity = CalculatePopularity(t).ToString(),
            ChildFriendliness = CalculateChildFriendliness(t).ToString(),
            Stops = string.IsNullOrWhiteSpace(t.Stops) ? new List<string>() : t.Stops.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            RouteGeometry = t.RouteGeometry
        };
    }

    public async Task<TourDto> CreateAsync(CreateTourDto dto)
    {
        double distance = 0;
        TimeSpan estimatedTime = TimeSpan.Zero;
        string routeGeometry = "";

        if (string.IsNullOrWhiteSpace(dto.TransportType))
        {
            dto.TransportType = TransportTypes.DrivingCar;
        }

        var route = await _ors.GetRouteAsync(dto.From, dto.To, dto.TransportType, dto.Stops ?? new List<string>());

        distance = route.Distance;
        estimatedTime = TimeSpan.FromMinutes(route.Duration);
        routeGeometry = route.Geometry;

        var userId = GetCurrentUserId();
        
        var tour = new Tour
        {
            Name = dto.Name,
            Description = dto.Description,
            From = dto.From,
            To = dto.To,
            TransportType = dto.TransportType,
            Distance = distance,
            EstimatedTime = estimatedTime,
            Stops = string.Join(",", dto.Stops ?? []),
            RouteGeometry = routeGeometry,
            UserId = userId
        };

        _context.Tours.Add(tour);
        await _context.SaveChangesAsync();

        return new TourDto
        {
            Id = tour.Id,
            Name = tour.Name,
            Description = tour.Description,
            From = tour.From,
            To = tour.To,
            TransportType = tour.TransportType,
            Distance = tour.Distance,
            EstimatedTime = tour.EstimatedTime.ToString(),
            Stops = string.IsNullOrWhiteSpace(tour.Stops) ? new List<string>() : tour.Stops.Split(',').ToList(),
            RouteGeometry = tour.RouteGeometry
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var userId = GetCurrentUserId();

        var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (tour == null) return false;

        _context.Tours.Remove(tour);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, CreateTourDto dto)
    {
        var userId = GetCurrentUserId();

        var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (tour == null) return false;

        var route = await _ors.GetRouteAsync(dto.From, dto.To, dto.TransportType, dto.Stops ?? new List<string>());

        tour.Name = dto.Name;
        tour.Description = dto.Description;
        tour.From = dto.From;
        tour.To = dto.To;
        tour.TransportType = dto.TransportType;
        tour.Stops = string.Join(",", dto.Stops ?? []);

        tour.Distance = route.Distance;
        tour.EstimatedTime = TimeSpan.FromMinutes(route.Duration);
        tour.RouteGeometry = route.Geometry;

        await _context.SaveChangesAsync();
        return true;
    }

    private int GetCurrentUserId()
    {
        var claim = _httpContextAccessor.HttpContext?
            .User
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

        if (claim == null)
            throw new UnauthorizedAccessException();

        return int.Parse(claim.Value);
    }

    private double CalculatePopularity(Tour tour)
    {
        //return tour.TourLogs?.Count ?? 0;
        //return tour.TourLogs?.Count() ?? 0;
        return (tour.TourLogs != null) ? tour.TourLogs.Count : 0;
    }

    private double CalculateChildFriendliness(Tour tour)
    {
        if (tour.TourLogs == null || !tour.TourLogs.Any())
            return 100;

        var avgDifficulty = tour.TourLogs.Average(l => l.Difficulty);
        var avgDistance = tour.TourLogs.Average(l => l.TotalDistance);
        var avgTime = tour.TourLogs.Average(l => l.TotalTime);

        var score = 100 - (avgDifficulty * 20 + avgDistance * 2 + avgTime * 1);

        return Math.Clamp(score, 0, 100);
    }
}