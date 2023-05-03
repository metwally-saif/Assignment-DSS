using Assignment_DSS.Interfaces;
using Assignment_DSS.modules;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Assignment_DSS.controllers
{

    public class AddController : Controller
    {

        private readonly IPostsRep _postsRep;
        private readonly ISession _session;


        public AddController(IPostsRep postsRep, IHttpContextAccessor httpContextAccessor, IUserRepo userRepo)

        {
            _session = httpContextAccessor.HttpContext.Session;
            _postsRep = postsRep;
        }

        public IActionResult Add()
        {
            if(string.IsNullOrEmpty(_session.GetString("Name")))
            {
                return RedirectToAction("Home", "Home");
            }
            var post = new Posts();
            return View(post);
        }

        [HttpPost]
        public ActionResult Add(string Title, string Img, string Body)
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Img) || string.IsNullOrEmpty(Body))
            {
                return View(new Posts());
            }
            string regexPattern = @"^(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|jpeg|png|gif)$";
            bool isValidImageLink = Regex.IsMatch(Img, regexPattern);
            if (!isValidImageLink) Img = "https://img.freepik.com/free-vector/glitch-error-404-page-background_23-2148090410.jpg";



            var UID = (int)_session.GetInt32("Id");
            
            var NPost = new Posts() { Title = Title, Img = Img, Body = Body, UserId = UID, Username = _session.GetString("Name").ToString() };
            _postsRep.Add(NPost);
            return RedirectToAction("Home", "Home");

        }
    }
}
