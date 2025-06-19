using System;
using System.Collections.Generic;
namespace Orders.Application.DTOs;

public record SubscriptionDto(
    Guid Id,
    Guid UserId,
    string Type,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    decimal Price);

public record CreateSubscriptionDto(
    Guid UserId,
    string Type,  // "Monthly" or "Yearly"
    decimal Price);

public record RenewSubscriptionDto(
    Guid SubscriptionId,
    string Type);