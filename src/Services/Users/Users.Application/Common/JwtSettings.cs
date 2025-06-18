namespace Users.Application.Common;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public int ExpiryDays { get; set; }
}