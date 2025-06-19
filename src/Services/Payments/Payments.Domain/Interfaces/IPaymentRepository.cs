using Payments.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payments.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> AddAsync(Payment payment);
        Task<Payment> GetByIdAsync(Guid id);
        Task<IEnumerable<Payment>> GetUserPaymentsAsync(Guid userId);
        Task UpdateAsync(Payment payment);
    }
}