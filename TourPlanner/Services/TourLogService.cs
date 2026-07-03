using Microsoft.EntityFrameworkCore;
using TourPlanner.Data;
using TourPlanner.DTOs;
using TourPlanner.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Services;

public class TourLogService : ITourLogService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TourLogService> _logger;

    public TourLogService(ApplicationDbContext context, ILogger<TourLogService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<TourLogDto>> GetByTourIdAsync(int tourId)
    {
        return await _context.TourLogs
            .Where(l => l.TourId == tourId)
            .Select(l => new TourLogDto
            {
                Id = l.Id,
                TourId = l.TourId,
                Date = l.Date,
                Comment = l.Comment,
                Difficulty = l.Difficulty,
                TotalDistance = l.TotalDistance,
                TotalTime = l.TotalTime,
                Rating = l.Rating
            })
            .ToListAsync();
    }

    public async Task<TourLogDto> CreateAsync(CreateTourLogDto dto)
    {
        //var date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc);    //used before
        //var date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc).ToUniversalTime(); //not really used beofre; maybe works better
        var date = DateTime.UtcNow; //used, because safer than receiving date from Frontend
        
        var log = new TourLog
        {
            TourId = dto.TourId,
            Date = date,
            Comment = dto.Comment,
            Difficulty = dto.Difficulty,
            TotalDistance = dto.TotalDistance,
            TotalTime = dto.TotalTime,
            Rating = dto.Rating
        };

        _context.TourLogs.Add(log);

        _logger.LogInformation("Creating tour log for TourId {TourId}", dto.TourId);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Tour log for TourId {TourId} created", dto.TourId);

        return new TourLogDto
        {
            Id = log.Id,
            TourId = log.TourId,
            Date = log.Date,
            Comment = log.Comment,
            Difficulty = log.Difficulty,
            TotalDistance = log.TotalDistance,
            TotalTime = log.TotalTime,
            Rating = log.Rating
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var log = await _context.TourLogs.FindAsync(id);
        if (log == null) return false;

        _context.TourLogs.Remove(log);

        _logger.LogWarning("Deleting tour log {Id}", id);

        await _context.SaveChangesAsync();

        _logger.LogWarning("Tour log {Id} deleted", id);

        return true;
    }

    public async Task<TourLogDto?> UpdateAsync(int id, CreateTourLogDto dto)
    {
        var log = await _context.TourLogs.FindAsync(id);
        if (log == null) return null;

        log.Date = dto.Date;
        log.Comment = dto.Comment;
        log.Difficulty = dto.Difficulty;
        log.TotalDistance = dto.TotalDistance;
        log.TotalTime = dto.TotalTime;
        log.Rating = dto.Rating;

        _logger.LogInformation("Updating tour log {Id}", id);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Tour log {Id} updated", id);

        return new TourLogDto
        {
            Id = log.Id,
            TourId = log.TourId,
            Date = log.Date,
            Comment = log.Comment,
            Difficulty = log.Difficulty,
            TotalDistance = log.TotalDistance,
            TotalTime = log.TotalTime,
            Rating = log.Rating
        };
    }
}