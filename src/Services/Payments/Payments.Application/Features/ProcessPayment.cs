using MediatR;
using Payments.Application.DTOs;
using Payments.Domain.Entities;
using Payments.Domain.Enums;
using Payments.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Features
{
    public static class ProcessPayment
    {
        public record Command(
            Guid UserId,
            Guid SubscriptionId,
            decimal Amount,
            string PaymentMethod) : IRequest<PaymentResult>;
        
        public class Handler : IRequestHandler<Command, PaymentResult>
        {
            private readonly IPaymentRepository _repository;

            public Handler(IPaymentRepository repository)
            {
                _repository = repository;
            }

            public async Task<PaymentResult> Handle(Command request, CancellationToken cancellationToken)
            {
                // Simulate payment processing
                await Task.Delay(500);
                
                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    SubscriptionId = request.SubscriptionId,
                    Amount = request.Amount,
                    PaymentMethod = request.PaymentMethod,
                    TransactionDate = DateTime.UtcNow,
                    Status = PaymentStatus.Completed,
                    TransactionId = $"SIM-{Guid.NewGuid()}"
                };
                
                var createdPayment = await _repository.AddAsync(payment);
                
                return new PaymentResult(
                    createdPayment.Id,
                    true,
                    createdPayment.TransactionDate);
            }
        }
        
        public record PaymentResult(
            Guid PaymentId,
            bool IsSuccess,
            DateTime TransactionDate);
    }
}