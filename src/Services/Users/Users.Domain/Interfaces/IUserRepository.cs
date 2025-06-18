using Users.Domain.Entities;

namespace Users.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task AddTokenToBlacklistAsync(BlacklistedToken token);
    Task<bool> IsTokenBlacklistedAsync(string token);
}