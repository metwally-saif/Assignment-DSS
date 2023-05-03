using Assignment_DSS.data;
using Assignment_DSS.modules;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Assignment_DSS.Repository;
using Assignment_DSS.Interfaces;

namespace Assignment_DSS.controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepo _userRepo;

        public LoginController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Name, string Password)
        {
            var Log = new User() { Name = Name, Password = Password  };


            var result = await _userRepo.GetByNameAndPass(Log);
            var LogDTO = new UserDTO() { Name = Name, Password = Password, cont = result.Count() };
            if(result.Count() == 0)
            {
                //TempData["fail"] = "user not found";
                return View(LogDTO);
            }
            else if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Name")))
            {
                return RedirectToAction("Home", "Home");
            }
            else
            {
                HttpContext.Session.SetString("Name", Log.Name);
                HttpContext.Session.SetInt32("Id", result.ToList()[0].Id);



                return RedirectToAction("Home", "Home");

            }

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Home", "Home");
        }


    }
}
