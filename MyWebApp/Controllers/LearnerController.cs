using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;

namespace MyWebApp.Controllers
{
    public class LearnerController : Controller
    {
        private readonly SchoolContext db;

        //khai báo biến toàn cục pageSize (Lab 5)
        private const int pageSize = 3;

        public LearnerController(SchoolContext context)
        {
            db = context;
        }

        // ================= R: danh sách + phân trang (Lab 4 bước 8.1, Lab 5, Lab 6) =================
        public IActionResult Index(int? mid)
        {
            // [BỔ SUNG] OrderBy: Skip/Take trong EF cần có sắp xếp để thứ tự trang ổn định
            var learners = db.Learners.Include(m => m.Major).OrderBy(l => l.LearnerID).AsQueryable();
            if (mid != null)
            {
                learners = learners.Where(l => l.MajorID == mid);
            }
            //tính số trang
            int pageNum = (int)Math.Ceiling(learners.Count() / (float)pageSize);
            //trả số trang về view để hiển thị nav-trang
            ViewBag.pageNum = pageNum;
            ViewBag.mid = mid;
            //lấy dữ liệu trang đầu
            var result = learners.Take(pageSize).ToList();
            return View(result);
        }

        // Lấy dữ liệu bằng AJAX: lọc + tìm kiếm + phân trang (Lab 5 bước 2)
        public IActionResult LearnerFilter(int? mid, string? keyword, int? pageIndex)
        {
            var learners = db.Learners.Include(m => m.Major).OrderBy(l => l.LearnerID).AsQueryable();
            //lấy chỉ số trang, nếu chỉ số trang null thì gán ngầm định bằng 1
            int page = (pageIndex == null || pageIndex <= 0) ? 1 : pageIndex.Value;
            //nếu có mid thì lọc learner theo mid (chuyên ngành)
            if (mid != null)
            {
                learners = learners.Where(l => l.MajorID == mid);
                ViewBag.mid = mid; //gửi mid về view để ghi lại trên nav-phân trang
            }
            //nếu có keyword thì tìm kiếm theo tên
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim().ToLower();
                learners = learners.Where(l => l.FirstMidName.ToLower().Contains(kw));
                ViewBag.keyword = keyword; //gửi keyword về view để ghi trên nav-phân trang
            }
            //tính số trang
            int pageNum = (int)Math.Ceiling(learners.Count() / (float)pageSize);
            ViewBag.pageNum = pageNum;
            //chọn dữ liệu trong trang hiện tại
            var result = learners.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            return PartialView("LearnerTable", result);
        }

        // Lab 6 (phần AJAX, chưa phân trang) - giữ lại để đối chiếu với đề
        public IActionResult LearnerByMajorID(int mid)
        {
            var learners = db.Learners
                .Where(l => l.MajorID == mid)
                .Include(m => m.Major).ToList();
            return PartialView("LearnerTable", learners);
        }

        // ================= C: thêm =================
        public IActionResult Create()
        {
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View(learner);
        }

        // ================= U: sửa =================
        public IActionResult Edit(int id)
        {
            var learner = db.Learners.Find(id);
            if (learner == null)
            {
                return NotFound();
            }
            // [SỬA LỖI ĐỀ] đề viết ViewBag.MajorId (chữ d thường) nhưng View dùng ViewBag.MajorID
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,
            [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (id != learner.LearnerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(learner);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        private bool LearnerExists(int id)
        {
            return db.Learners.Any(e => e.LearnerID == id);
        }

        // ================= D: xóa =================
        public IActionResult Delete(int id)
        {
            var learner = db.Learners
                .Include(l => l.Major)
                .Include(e => e.Enrollments)
                .FirstOrDefault(m => m.LearnerID == id);
            if (learner == null)
            {
                return NotFound();
            }
            if (learner.Enrollments.Count() > 0)
            {
                return Content("This learner has some enrollments, can't delete!");
            }
            return View(learner);
        }

        // POST: Learner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var learner = db.Learners.Find(id);
            if (learner != null)
            {
                db.Learners.Remove(learner);
            }
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
