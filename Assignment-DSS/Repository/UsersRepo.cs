using Assignment_DSS.data;
using Assignment_DSS.Interfaces;
using Assignment_DSS.modules;
using Microsoft.EntityFrameworkCore;

namespace Assignment_DSS.Repository
{
    public class UsersRepo : IUserRepo
    {
        private readonly ApplicationDBContext _context;

        public UsersRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public bool Add(User user)
        {
            _context.User.Add(user);
            return Save();
        }

        public bool Delete(User user)
        {
            _context.User.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<User>> GetByNameAndPass(User user)
        {
            var result = await _context.User.Where(x => x.Name == user.Name && x.Password == user.Password).ToListAsync();
            return result;
        }
        public async Task<User> GetUserAsync(int Id)
        {
            var result = await _context.User.FirstOrDefaultAsync(x => x.Id == Id);
            
            return result;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
