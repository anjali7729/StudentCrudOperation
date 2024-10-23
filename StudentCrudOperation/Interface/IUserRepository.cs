using StudentCrudOperation.Models;
using System.Threading.Tasks;

namespace StudentCrudOperation.Interface
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(User user);
        Task<User> LoginUser(string email, string password);
        Task<List<User>> GetUserList();
    }
}
