using Assignment_DSS.Interfaces;
using Assignment_DSS.modules;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Assignment_DSS.controllers
{
    public class RegController : Controller
    {
        private readonly IUserRepo _userRepo;

        public RegController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        public IActionResult Reg()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reg(string Name, string Password)
        {
            var Log = new User() { Name = Name, Password = Password };

            var result = await _userRepo.GetByNameAndPass(Log);

            Regex regex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$");

            if (result.Count() == 1)
            {
                TempData["fail"] = "User already exists";
                return View(Log);
            }
            else if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Password))
            {
                TempData["failEName"] = "The name field can't be empty";
                TempData["failEPass"] = "This Password can't be empty";
                return View(Log);
            }
            else if (!regex.IsMatch(Log.Password))
            {
                TempData["failEPass"] = "Password is too weak!";
                return View(Log);
            }
            else if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Name")))
            {
                return RedirectToAction("Home", "Home");
            }
            else
            {
                _userRepo.Add(Log);

                HttpContext.Session.SetString("Name", Log.Name);
                int id = result?.FirstOrDefault()?.Id ?? 0;
                HttpContext.Session.SetInt32("Id", id);


                TempData["userCreated"] = "<script>alert('User has been succesfully created');</script>";
                return RedirectToAction("Home", "Home");

            }

        }
    }
}
