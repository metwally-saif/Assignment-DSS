using Assignment_DSS.modules;

namespace Assignment_DSS.Interfaces
{
    public interface IUserRepo
    {
        Task<IEnumerable<User>> GetByNameAndPass(User user);
        Task<User> GetUserAsync(int Id);

        bool Add(User user);
        bool Delete(User user);
        bool Save();
    }
}
