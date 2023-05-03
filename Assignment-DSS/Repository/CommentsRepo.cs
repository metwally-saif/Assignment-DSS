using Assignment_DSS.data;
using Assignment_DSS.Interfaces;
using Assignment_DSS.modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Assignment_DSS.Repository
{
    public class CommentsRepo : ICommentsRep
    {
        private readonly ApplicationDBContext _context;
        private readonly IPostsRep _postsRep;

        public CommentsRepo(ApplicationDBContext context, IPostsRep postsRep)
        {
            _context = context;
            _postsRep = postsRep;   
        }

        public bool Add(Comment comment)
        {
            _context.Comments.Add(comment);
            return Save();
        }

        public bool Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
            return Save();
        }

        public async Task<IEnumerable<Comment>> GetAll(int postId)
        {
            var comments = await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
            return comments;
        }

        public async Task<Comment> GetByIdAsync(int Id)
        {
            Comment comment = await _context.Comments.FirstOrDefaultAsync(i => i.Id == Id);
            return comment;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
