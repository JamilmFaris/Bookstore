namespace Users.Domain.Entities;

public class BlacklistedToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expiry { get; set; }
}