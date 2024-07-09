using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using book_store_be.Models;

namespace book_store_be.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookModel>> GetAllBooksAsync();
        Task<BookModel> GetBookByIdAsync(Guid id);
        Task CreateBookAsync(BookModel book);
        Task UpdateBookAsync(BookModel book);
        Task DeleteBookAsync(Guid id);
    }
}
