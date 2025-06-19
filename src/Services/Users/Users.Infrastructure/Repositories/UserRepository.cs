using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Users.Domain.Entities;
using Users.Domain.Interfaces;
using Users.Infrastructure.Data;
using System;
using System.Collections.Generic;
namespace Users.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
        => await _context.Users.FindAsync(id);

    public async Task<User?> GetByUsernameAsync(string username)
        => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task AddTokenToBlacklistAsync(BlacklistedToken token)
    {
        await _context.BlacklistedTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsTokenBlacklistedAsync(string token)
    {
        return await _context.BlacklistedTokens
            .AnyAsync(t => t.Token == token && t.Expiry > DateTime.UtcNow);
    }
}