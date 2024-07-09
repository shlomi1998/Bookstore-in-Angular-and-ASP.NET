using System.Threading.Tasks;
using book_store_be.Models;

namespace book_store_be.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> UserExistsAsync(string email);
        Task<UserModel> GetUserByEmailAsync(string email);
        Task<UserModel> GetUserByIdAsync(int id);
        Task<UserModel> GetUserByUserNameAsync(string userName);
        Task AddUserAsync(UserModel user);
    }
}
