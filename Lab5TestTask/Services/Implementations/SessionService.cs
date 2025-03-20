using Lab5TestTask.Data;
using Lab5TestTask.Enums;
using Lab5TestTask.Models;
using Lab5TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab5TestTask.Services.Implementations;

/// <summary>
/// SessionService implementation.
/// Implement methods here.
/// </summary>
public class SessionService : ISessionService
{
    private readonly ApplicationDbContext _dbContext;

    public SessionService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Session> GetSessionAsync()
    {
        return await _dbContext.Sessions
            .Where(s => s.DeviceType == DeviceType.Desktop)
            .OrderBy(s => s.StartedAtUTC)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Session>> GetSessionsAsync()
    {
        var sessions = await _dbContext.Sessions
            .Include(s => s.User)
            .Where(s => s.User.Status == Lab5TestTask.Enums.UserStatus.Active &&
                        s.EndedAtUTC.Year < 2025)
            .ToListAsync();

        foreach (var session in sessions)
        {
            if (session.User != null)
            {
                session.User.Sessions = null;
            }
        }

        return sessions;
    }
}
