using Lab5TestTask.Data;
using Lab5TestTask.Enums;
using Lab5TestTask.Models;
using Lab5TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab5TestTask.Services.Implementations;

/// <summary>
/// UserService implementation.
/// Implement methods here.
/// </summary>
public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;

    public UserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User> GetUserAsync()
    {
        var user = await _dbContext.Users
            .Include(u => u.Sessions)
            .OrderByDescending(u => u.Sessions.Count)
            .FirstOrDefaultAsync();

        if (user != null && user.Sessions != null)
        {
            foreach (var session in user.Sessions)
            {
                session.User = null;
            }
        }

        return user;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var users = await _dbContext.Users
            .Include(u => u.Sessions)
            .Where(u => u.Sessions.Any(s => s.DeviceType == DeviceType.Mobile))
            .ToListAsync();

        foreach (var user in users)
        {
            if (user.Sessions != null)
            {
                foreach (var session in user.Sessions)
                {
                    session.User = null;
                }
            }
        }

        return users;
    }
}
