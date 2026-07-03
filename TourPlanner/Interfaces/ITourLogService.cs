using TourPlanner.DTOs;

namespace TourPlanner.Interfaces;

public interface ITourLogService
{
    Task<List<TourLogDto>> GetByTourIdAsync(int tourId);
    Task<TourLogDto> CreateAsync(CreateTourLogDto dto);
    Task<bool> DeleteAsync(int id);
    Task<TourLogDto?> UpdateAsync(int id, CreateTourLogDto dto);
}