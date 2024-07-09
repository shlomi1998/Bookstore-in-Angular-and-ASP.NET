using Microsoft.EntityFrameworkCore;
using book_store_be.Models;

namespace book_store_be.Data
{
    public class BookStoreDbContext :DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<BookModel> Books { get; set; }

    }
}


