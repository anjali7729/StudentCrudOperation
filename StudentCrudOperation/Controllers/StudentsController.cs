using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentCrudOperation.Data;
using StudentCrudOperation.Interface;
using StudentCrudOperation.Models;
using StudentCrudOperation.Repository;

namespace StudentCrudOperation.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _studentService = studentService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            try
            {
                var student = await _studentService.GetStudentList();
                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while [Action Name]: {Message}", ex.Message);
                return View("Error");
            }
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentService.GetStudentById(id);
         
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            var userEmail = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Student student, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                if (ImageFile != null)
                {
                    var request = _httpContextAccessor.HttpContext.Request;
                    string currentUrl = $"{request.Scheme}://{request.Host}";

                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath,"Images\\" + fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    student.ImageFile = $"{currentUrl}/Images/{fileName}";

                }
                //var existingStudents = await _studentService.GetStudentList();
                //if (existingStudents.Any(s => s.Email == student.Email))
                //{
                //    ModelState.AddModelError("Email", "Email already exists.");
                //    return View(student);
                //}
                await _studentService.AddStudent(student);
                    return RedirectToAction(nameof(Index));
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while [Action Name]: {Message}", ex.Message);
                return View("Error");
            }

        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var userEmail = HttpContext.Session.GetString("UserName");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return RedirectToAction("Index", "Home");
                }

                var student = await _studentService.GetStudentById(id);
                if (student == null)
                {
                    return NotFound();
                }
                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while [Action Name]: {Message}", ex.Message);
                return View("Error");
            }
           
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student,IFormFile ImageFile)
        {

            if (id != student.Id)
            {
                return BadRequest();
            }
            try
            {
                if (ImageFile != null)
                {
                    var request = _httpContextAccessor.HttpContext.Request;
                    string currentUrl = $"{request.Scheme}://{request.Host}";

                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images\\" + fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    student.ImageFile = $"{currentUrl}/Images/{fileName}";

                }
                else
                {
                    var existingBlog = await _studentService.GetStudentById(student.Id);
                    student.ImageFile = existingBlog.ImageFile;
                }



                    await _studentService.UpdateStudent(id, student);
                    return RedirectToAction(nameof(Index));
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while [Action Name]: {Message}", ex.Message);
                return View("Error");
            }
            
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userEmail = HttpContext.Session.GetString("UserName");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return RedirectToAction("Index", "Home");
                }

                var student = await _studentService.GetStudentById(id);

                if (student == null)
                {
                    return NotFound();
                }

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while [Action Name]: {Message}", ex.Message);
                return View("Error");
            }
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try {
                await _studentService.DeleteStudent(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while [Action Name]: {Message}", ex.Message);
                return View("Error");
            }
        }
        //[HttpPost]
        //public JsonResult CheckEmail(string email)
        //{
        //    var existingStudent = _studentService.GetStudentList().Result;
        //    var isEmailExists = existingStudent.Any(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        //    return Json(!isEmailExists); 
        //}

        [HttpPost]
        public JsonResult CheckEmail(string email, int? currentUserId)
        {
            var existingStudent = _studentService.GetStudentList().Result;
            var isEmailExists = existingStudent.Any(s => s.Email == email && s.Id != currentUserId);
            return Json(!isEmailExists);
        }
    }
}
