using AutoMapper;
using MediatR;
using Orders.Application.Commands;
using Orders.Application.DTOs;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System;
public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, SubscriptionDto>
{
    private readonly ISubscriptionRepository _repository;
    private readonly IMapper _mapper;

    public CreateSubscriptionCommandHandler(ISubscriptionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SubscriptionDto> Handle(CreateSubscriptionCommand request, CancellationToken ct)
    {
        var subscription = new Subscription
        {
            UserId = request.Dto.UserId,
            Type = Enum.Parse<SubscriptionType>(request.Dto.Type),
            Price = request.Dto.Price,
            EndDate = DateTime.UtcNow.AddMonths((int)Enum.Parse<SubscriptionType>(request.Dto.Type))
        };

        await _repository.AddAsync(subscription);
        return _mapper.Map<SubscriptionDto>(subscription);
    }
}

public class RenewSubscriptionCommandHandler : IRequestHandler<RenewSubscriptionCommand, SubscriptionDto>
{
    private readonly ISubscriptionRepository _repository;
    private readonly IMapper _mapper;

    public RenewSubscriptionCommandHandler(ISubscriptionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SubscriptionDto> Handle(RenewSubscriptionCommand request, CancellationToken ct)
    {
        var subscription = await _repository.GetByIdAsync(request.Dto.SubscriptionId);
        if (subscription == null) throw new Exception("Subscription not found");

        subscription.Type = Enum.Parse<SubscriptionType>(request.Dto.Type);
        subscription.StartDate = DateTime.UtcNow;
        subscription.EndDate = DateTime.UtcNow.AddMonths((int)subscription.Type);
        subscription.IsActive = true;

        await _repository.UpdateAsync(subscription);
        return _mapper.Map<SubscriptionDto>(subscription);
    }
}