using Assignment_DSS.modules;

namespace Assignment_DSS.Interfaces
{
    public interface ICommentsRep
    {
        Task<IEnumerable<Comment>> GetAll(int postId);
        Task<Comment> GetByIdAsync(int UserId);
        bool Add(Comment comment);
        bool Delete(Comment comment);
        bool Save();
    }
}
