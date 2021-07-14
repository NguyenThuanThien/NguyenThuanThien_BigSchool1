using Microsoft.AspNet.Identity;
using NguyenThuanThien_BigSchool.Models;
using NguyenThuanThien_BigSchool.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace NguyenThuanThien_BigSchool.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _dbConText;

        public CoursesController()
        {
            _dbConText = new ApplicationDbContext();
        }

        // GET: Courses

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dbConText.Categories.ToList(),

            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbConText.Categories.ToList();
                return View("Create", viewModel);
            }
            var course = new Course
            {
               LecturerId = User.Identity.GetUserId(),
               DateTime = viewModel.GetDateTime(),
               CategoryId = viewModel.Category,
               Place = viewModel.Place
            };
            _dbConText.Courses.Add(course);
            _dbConText.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Attending ()
        {
            var userId = User.Identity.GetUserId();

            var courses = _dbConText.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include(l => l.Lecturer)
                .Include(l => l.Category)
                .ToList();

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);
        }
    }
    

}