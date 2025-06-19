using MediatR;
using Payments.Application.DTOs;
using Payments.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Features
{
    public static class GetPaymentHistory
    {
        public record Query(Guid UserId) : IRequest<IEnumerable<PaymentDto>>;
        
        public class Handler : IRequestHandler<Query, IEnumerable<PaymentDto>>
        {
            private readonly IPaymentRepository _repository;

            public Handler(IPaymentRepository repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<PaymentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var payments = await _repository.GetUserPaymentsAsync(request.UserId);
                
                return payments.Select(p => new PaymentDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    SubscriptionId = p.SubscriptionId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    TransactionDate = p.TransactionDate,
                    Status = p.Status.ToString(),
                    TransactionId = p.TransactionId
                });
            }
        }
    }
}