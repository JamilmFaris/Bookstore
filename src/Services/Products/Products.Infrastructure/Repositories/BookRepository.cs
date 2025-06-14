using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using Products.Infrastructure.Data;

namespace Products.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookstoreContext _context;

    public BookRepository(BookstoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
        => await _context.Books.ToListAsync();

    public async Task<Book> GetByIdAsync(int id)
        => await _context.Books.FindAsync(id);

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await GetByIdAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}