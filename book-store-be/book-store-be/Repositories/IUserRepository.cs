using System;
using System.Threading.Tasks;
using book_store_be.Models;

namespace book_store_be.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> AddUserAsync(UserModel user);
        Task<UserModel> GetUserByIdAsync(int id);
        Task<UserModel> AddBookToCartAsync(int userId, Guid bookId);
        Task<UserModel> RemoveBookFromCartAsync(int userId, Guid bookId);
        Task UpdateUserAsync(UserModel user);
        Task<int> GetCartItemCountAsync(int userId);
    }
}
