namespace Users.Application.DTOs;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    bool IsSubscribed,
    DateTime? SubscriptionExpiry);

public record RegisterUserDto(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName);

public record LoginDto(
    string Username,
    string Password);

public record AuthResponse(
    string Token,
    UserDto User);

public record UpdateUserDto(
    string FirstName,
    string LastName,
    string Email);

public record SubscriptionDto(
    bool Subscribe,
    int Months = 1);