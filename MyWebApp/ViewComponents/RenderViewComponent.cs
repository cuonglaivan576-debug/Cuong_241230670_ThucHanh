using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;

namespace MyWebApp.ViewComponents
{
    // Lab 2 bước 5: Strongly Typed PartialView cho menu trái
    public class RenderViewComponent : ViewComponent
    {
        private List<MenuItem> MenuItems = new List<MenuItem>();

        public RenderViewComponent()
        {
            // [BỔ SUNG] đề để link "Branches/List", "Students/List"... là link giả (chưa có controller).
            // Mình đổi sang các trang đang có thật; Subjects/Courses để "#".
            MenuItems = new List<MenuItem>() {
                new MenuItem() {Id=1, Name="Branches", Link="/Learner" },
                new MenuItem() {Id=2, Name="Students", Link="/Admin/Student/List" },
                new MenuItem() {Id=3, Name="Subjects", Link="#"},
                new MenuItem() {Id=4, Name="Courses", Link="#"}
            };
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View("RenderLeftMenu", MenuItems));
        }
    }
}
