using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;

namespace MyWebApp.ViewComponents
{
    // Lab 6: hiển thị danh sách chuyên ngành để lọc Learner
    public class MajorViewComponent : ViewComponent
    {
        private readonly SchoolContext db;

        public MajorViewComponent(SchoolContext _context)
        {
            db = _context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var majors = await db.Majors.ToListAsync();
            return View("RenderMajor", majors);
        }
    }
}
