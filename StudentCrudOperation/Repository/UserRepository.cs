using Microsoft.EntityFrameworkCore;
using StudentCrudOperation.Data;
using StudentCrudOperation.Interface;
using StudentCrudOperation.Models;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace StudentCrudOperation.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext applicationDbContext) {
            _context = applicationDbContext;
        }

        public async Task<User> RegisterUser(User user)
        {
            var users = new User()
            {
                FullName = user.FullName,
                Email = user.Email,
                password = HashPassword(user.password),                
            };
            _context.users.Add(users);
            await _context.SaveChangesAsync();
            return user;
        }
        
        public async Task<User> LoginUser(string email, string password)
        {
            var user = await _context.users.Where(r => r.Email == email).FirstOrDefaultAsync();
            if (user != null && VerifyPassword(user.password,password))
            { 
                return user;
            }
            return null;
        }
        private string HashPassword(string password) 
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
        private bool VerifyPassword(string storePassword,string password)
        {
            string shaPassword = HashPassword(password);
            return storePassword == shaPassword;
        }
        public async Task<List<User>> GetUserList()
        {
            return await _context.users.ToListAsync();
        }
    }
}
