using Assignment_DSS.data;
using Assignment_DSS.Interfaces;
using Assignment_DSS.modules;
using Assignment_DSS.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Reflection.Metadata;

namespace Assignment_DSS.controllers
{
    public class HomeController : Controller

    {

        private readonly IPostsRep _postsRep;
        private readonly IUserRepo _usersRepo;
        private readonly ICommentsRep _commentRep;



        public HomeController(IPostsRep postsRep, IUserRepo usersRepo, ICommentsRep commentRep)

        {
            _postsRep = postsRep;
            _usersRepo = usersRepo;
            _commentRep = commentRep;
        }


        public async Task<IActionResult> Home()
        {

            var posts = await _postsRep.GetAll();
            return View(posts);
        }

        public async Task<IActionResult> Post(int Id)
        {
            CommentAndContentDTO commentAndContent = new();
            Posts posts = await _postsRep.GetByIdAsync(Id);
            commentAndContent.blog = posts;
            var AllComments = await _commentRep.GetAll(Id) ;
            if (AllComments != null) commentAndContent.AllComments = AllComments;
            if (commentAndContent == null)
            {
                RedirectToAction("Home");
            }
            return View(commentAndContent);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int Id, string CommentbBody)
        {
            Comment comment = new Comment() { body = CommentbBody, PostId = Id, UserId = HttpContext.Session.GetInt32("Id").Value, UserName = HttpContext.Session.GetString("Name") };
            _commentRep.Add(comment);
            
            return RedirectToAction("Post", new { Id = Id });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int Id, int ComId)
        {
            var comment = await _commentRep.GetByIdAsync(ComId);
             _commentRep.Delete(comment);
            return RedirectToAction("Post", new { Id = Id });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int Id)
        {
            var AllComments = await _commentRep.GetAll(Id);
            foreach (var i in AllComments)
            {
                _commentRep.Delete(i);
            }

            var blog = await _postsRep.GetByIdAsync(Id);
            _postsRep.Delete(blog);

            return RedirectToAction("Home");
        }
        
    }
}
