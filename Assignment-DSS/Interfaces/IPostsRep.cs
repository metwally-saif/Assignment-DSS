using Assignment_DSS.modules;

namespace Assignment_DSS.Interfaces
{

    public interface IPostsRep
    {
        Task<IEnumerable<Posts>> GetAll();
        Task<Posts> GetByIdAsync(int id);
        bool Add(Posts post);
        bool Delete(Posts post);
        bool Save();
    }
}
