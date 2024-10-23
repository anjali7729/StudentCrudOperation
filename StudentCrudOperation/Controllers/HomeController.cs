using Microsoft.AspNetCore.Mvc;
using StudentCrudOperation.Interface;
using StudentCrudOperation.Models;
using StudentCrudOperation.Services;
using System.Diagnostics;

namespace StudentCrudOperation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IUserRepository userRepository, ILogger<HomeController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public IActionResult Index()
       {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                var user = await _userRepository.LoginUser(email,password);
                if (user != null) {

                    HttpContext.Session.SetString("UserID", user.Id.ToString());
                    HttpContext.Session.SetString("UserName", user.FullName);
                    return RedirectToAction("Index", "Students");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex) {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if(!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        // state.Key gives the property name
                        // state.Value.Errors contains the error messages for the invalid property
                        foreach (var error in state.Value.Errors)
                        {
                            // Log the error or handle it
                            Console.WriteLine($"Invalid property: {state.Key}, Error: {error.ErrorMessage}");
                        }
                    }
                }

            }
            try
            {
                await _userRepository.RegisterUser(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while [Action Name]: {Message}", ex.Message);
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Index));
        }
        //[HttpPost]
        //public JsonResult CheckEmail(string email, Guid? currentUserId)
        //{
        //    var existingUser = _userRepository.GetUserList().Result;
        //    var isEmailExists = existingUser.Any(s => s.Email == email && s.Id == currentUserId);
        //    return Json(!isEmailExists);
        //}

        //[HttpPost]
        //public JsonResult CheckEmail(string Email)
        //{
        //    var existingStudent = _userRepository.GetUserList().Result;
        //    var isEmailExists = existingStudent.Any(s => s.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));
        //    return Json(!isEmailExists);
        //}

        [HttpPost]
        public JsonResult CheckEmail(string email, Guid? currentUserId = null)
        {
            var existingUsers = _userRepository.GetUserList().Result;

            var isEmailExists = existingUsers.Any(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                            && (!currentUserId.HasValue || s.Id != currentUserId.Value));

            return Json(!isEmailExists);
        }

    }
}
