using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using TourPlanner.Data;
using TourPlanner.Services;
using TourPlanner.Models;
using TourPlanner.DTOs;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace TourPlanner.Tests;

[TestFixture]
public class TourLogServiceTests
{
    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
    }
    
    private ApplicationDbContext _context;
    private TourLogService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _service = new TourLogService(_context);
    }

    [Test]
    public async Task Create_ShouldAddLog()
    {
        var dto = new CreateTourLogDto
        {
            TourId = 1,
            Comment = "Nice",
            Difficulty = 2,
            TotalDistance = 10,
            TotalTime = 60,
            Rating = 4,
            Date = DateTime.UtcNow
        };

        var result = await _service.CreateAsync(dto);

        Assert.That(result.Id, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetByTourId_ShouldReturnLogs()
    {
        await CreateSampleLog();

        var result = await _service.GetByTourIdAsync(1);

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task Delete_ShouldRemoveLog()
    {
        var log = await CreateSampleLog();

        var result = await _service.DeleteAsync(log.Id);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task Delete_ShouldReturnFalse_WhenMissing()
    {
        var result = await _service.DeleteAsync(999);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task Update_ShouldModifyLog()
    {
        var log = await CreateSampleLog();

        var dto = new CreateTourLogDto
        {
            TourId = 1,
            Comment = "Updated",
            Difficulty = 5,
            TotalDistance = 20,
            TotalTime = 120,
            Rating = 3,
            Date = DateTime.UtcNow
        };

        var result = await _service.UpdateAsync(log.Id, dto);

        Assert.That(result!.Comment, Is.EqualTo("Updated"));
    }

    private async Task<TourLog> CreateSampleLog()
    {
        var log = new TourLog
        {
            TourId = 1,
            Comment = "Test",
            Difficulty = 2,
            TotalDistance = 10,
            TotalTime = 60,
            Rating = 4,
            Date = DateTime.UtcNow
        };

        _context.TourLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }
}