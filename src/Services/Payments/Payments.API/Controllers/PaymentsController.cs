using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.DTOs;
using Payments.Application.Features;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payments.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ProcessPayment.PaymentResult>> ProcessPayment(
            [FromBody] ProcessPayment.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("history/{userId}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentHistory(Guid userId)
        {
            var payments = await _mediator.Send(new GetPaymentHistory.Query(userId));
            return Ok(payments);
        }

        [HttpPost("confirm/{paymentId}")]
        public async Task<ActionResult<PaymentConfirmationResult>> ConfirmPayment(Guid paymentId)
        {
            // In a real implementation, this would call the Orders service
            // For simulation, we'll just return a success response
            return Ok(new PaymentConfirmationResult(
                paymentId,
                Guid.NewGuid(), // Subscription ID
                DateTime.UtcNow.AddMonths(1))); // Simulated expiry date
        }
        
        public record PaymentConfirmationResult(
            Guid PaymentId,
            Guid SubscriptionId,
            DateTime ExpiryDate);
    }
}