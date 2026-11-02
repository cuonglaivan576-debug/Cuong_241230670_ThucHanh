using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;

namespace MyWebApp.Controllers
{
    public class StudentController : Controller
    {
        private static readonly List<Student> listStudents = new List<Student>();
        private readonly IWebHostEnvironment _env;

        public StudentController(IWebHostEnvironment env)
        {
            _env = env;
            if (listStudents.Count == 0)
            {
                listStudents.AddRange(new List<Student>()
                {
                    new Student() { Id = 101, Name = "Hải Nam", Branch = Branch.IT,
                        Gender = Gender.Male, IsRegular = true,
                        Address = "A1-2018", Email = "nam@g.com",
                        DateOfBorth = new DateTime(2000, 1, 1), Diem = 8.0 },
                    new Student() { Id = 102, Name = "Minh Tú", Branch = Branch.BE,
                        Gender = Gender.Female, IsRegular = true,
                        Address = "A1-2019", Email = "tu@g.com",
                        DateOfBorth = new DateTime(2001, 2, 2), Diem = 7.5 },
                    new Student() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE,
                        Gender = Gender.Male, IsRegular = false,
                        Address = "A1-2020", Email = "phong@g.com",
                        DateOfBorth = new DateTime(2002, 3, 3), Diem = 6.0 },
                    new Student() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE,
                        Gender = Gender.Female, IsRegular = false,
                        Address = "A1-2021", Email = "mai@g.com",
                        DateOfBorth = new DateTime(2003, 4, 4), Diem = 9.0 }
                });
            }
        }

        public IActionResult Index()
        {
            return View(listStudents);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateLists();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student s, IFormFile? avatarFile)
        {
            if (avatarFile != null && avatarFile.Length > 0)
            {
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();
                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("Avatar", "Chỉ chấp nhận file ảnh .jpg, .jpeg, .png, .gif");
                }
                else if (avatarFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Avatar", "Ảnh không được lớn hơn 2MB");
                }
                else if (ModelState.IsValid)
                {
                    var folder = Path.Combine(_env.WebRootPath, "uploads");
                    Directory.CreateDirectory(folder);
                    var fileName = Guid.NewGuid().ToString("N") + ext; 
                    using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(stream);
                    }
                    s.Avatar = "/uploads/" + fileName;
                }
            }

            if (ModelState.IsValid)
            {
                s.Id = listStudents.Max(x => x.Id) + 1;
                listStudents.Add(s);
                return RedirectToAction("Index");
            }

            PopulateLists();
            return View(s);
        }

        private void PopulateLists()
        {
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            ViewBag.AllBranches = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>()
            {
                new() { Text = "IT", Value = "1" },
                new() { Text = "BE", Value = "2" },
                new() { Text = "CE", Value = "3" },
                new() { Text = "EE", Value = "4" }
            };
        }
    }
}
