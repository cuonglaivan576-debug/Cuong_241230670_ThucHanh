using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;

namespace MyWebApp.Controllers
{
    public class StudentController : Controller
    {
        // [BỔ SUNG] để static: mỗi request MVC tạo controller mới, nếu không static thì
        // sinh viên vừa thêm sẽ mất ở lần tải tiếp theo.
        private static readonly List<Student> listStudents = new List<Student>();
        private readonly IWebHostEnvironment _env;

        public StudentController(IWebHostEnvironment env)
        {
            _env = env;
            //Tạo danh sách sinh viên với 4 dữ liệu mẫu (Lab 1 bước 4)
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
            //Trả về View Index.cshtml cùng Model là danh sách sv listStudents
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
            // [BỔ SUNG - Lab 1 bước 9] upload ảnh đại diện
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
                    var fileName = Guid.NewGuid().ToString("N") + ext; // đặt tên mới để tránh trùng/đường dẫn độc hại
                    using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(stream);
                    }
                    s.Avatar = "/uploads/" + fileName;
                }
            }

            // Lab 3 bước 3: chỉ thêm khi dữ liệu hợp lệ
            if (ModelState.IsValid)
            {
                s.Id = listStudents.Max(x => x.Id) + 1;
                listStudents.Add(s);
                return RedirectToAction("Index");
            }

            // dữ liệu sai -> nạp lại danh sách cho radio/select và hiện lại form kèm lỗi
            PopulateLists();
            return View(s);
        }

        private void PopulateLists()
        {
            //lấy danh sách các giá trị Gender để hiển thị radio button trên form
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            //lấy danh sách các giá trị Branch để hiển thị select-option trên form
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
