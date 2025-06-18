// Products.Domain/Interfaces/IBookRepository.cs
using Products.Domain.Entities;

namespace Products.Domain.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book> GetByIdAsync(int id);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
}