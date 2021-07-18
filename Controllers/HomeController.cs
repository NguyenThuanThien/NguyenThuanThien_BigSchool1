using NguyenThuanThien_BigSchool.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using NguyenThuanThien_BigSchool.ViewModels;
using Microsoft.AspNet.Identity;

namespace NguyenThuanThien_BigSchool.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _dbContext;
        public HomeController()
        {
            _dbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var upcomingCourses = _dbContext.Courses
                .Include(c => c.Lecturer)
                .Include(c => c.Category)
                .Where(x=>x.IsCanceled==false)
                .Where(c => c.DateTime > DateTime.Now);
            var userId = User.Identity.GetUserId();
            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = upcomingCourses,
                ShowAction = User.Identity.IsAuthenticated,
                Followings = _dbContext.Followings.Where(f => userId != null && f.FollowerId == userId).ToList(),
                Attendances = _dbContext.Attendances.Where(f => userId != null && f.AttendeeId == userId).ToList()
            };
            return View(viewModel);
        }


    }
}