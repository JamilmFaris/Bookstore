using System;
using Payments.Domain.Enums;
namespace Payments.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime TransactionDate { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }
    }
}