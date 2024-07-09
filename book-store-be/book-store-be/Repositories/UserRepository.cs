using book_store_be.Data;
using book_store_be.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace book_store_be.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreDbContext _context;

        public UserRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel> AddUserAsync(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            return await _context.Users.Include(u => u.Cart).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserModel> AddBookToCartAsync(int userId, Guid bookId)
        {
            var user = await _context.Users.Include(u => u.Cart).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            user.Cart.Add(book);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel> RemoveBookFromCartAsync(int userId, Guid bookId)
        {
            var user = await _context.Users.Include(u => u.Cart).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var book = user.Cart.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                throw new Exception("Book not found in cart");
            }

            user.Cart.Remove(book);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(UserModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCartItemCountAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.Cart).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user.Cart.Count;
        }
    }
}
