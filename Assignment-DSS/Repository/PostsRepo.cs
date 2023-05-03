using Assignment_DSS.data;
using Assignment_DSS.Interfaces;
using Assignment_DSS.modules;
using Microsoft.EntityFrameworkCore;

namespace Assignment_DSS.Repository
{
    public class PostsRepo : IPostsRep
    {
        private readonly ApplicationDBContext _context;
        private readonly IUserRepo _userRepo;

        public PostsRepo(ApplicationDBContext context, IUserRepo userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }
        public bool Add(Posts post)
        {
            _context.Posts.Add(post);
            return Save();
        }


        public bool Delete(Posts post)
        {
            _context.Posts.Remove(post);
            return Save();
        }

        public async Task<IEnumerable<Posts>> GetAll()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Posts> GetByIdAsync(int id)
        {
            Posts posts = await _context.Posts.FirstOrDefaultAsync(i => i.Id == id);
            return posts;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

    }
}
