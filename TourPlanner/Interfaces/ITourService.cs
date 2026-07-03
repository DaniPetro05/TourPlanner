using TourPlanner.DTOs;

namespace TourPlanner.Interfaces;

public interface ITourService
{
    Task<List<TourDto>> GetAllAsync();
    Task<TourDto?> GetByIdAsync(int id);
    Task<TourDto> CreateAsync(CreateTourDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(int id, CreateTourDto dto);
}