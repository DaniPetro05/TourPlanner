using Microsoft.EntityFrameworkCore;
using TourPlanner.Data;
using TourPlanner.DTOs;
using TourPlanner.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Services;

public class TourLogService : ITourLogService
{
    private readonly ApplicationDbContext _context;

    public TourLogService(ApplicationDbContext context)
    {
        _context = context;
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
        await _context.SaveChangesAsync();

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
        await _context.SaveChangesAsync();
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

        await _context.SaveChangesAsync();

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