using Microsoft.AspNet.Identity;
using NguyenThuanThien_BigSchool.Models;
using NguyenThuanThien_BigSchool.ViewModels;
using System;
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
                Heading = "Add Course"

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

            return RedirectToAction("Mine", "Courses");
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
                ShowAction = User.Identity.IsAuthenticated,
                Followings = _dbConText.Followings.Where(f => userId != null && f.FolloweeId == userId).ToList(),
                Attendances = _dbConText.Attendances.Include(a => a.Course).ToList()
            };
            return View(viewModel);
        }

        [Authorize]
        public ActionResult Mine ()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbConText.Courses
                .Where(c => c.LecturerId == userId && c.DateTime > DateTime.Now && c.IsCanceled==false)
                .Include(l => l.Lecturer)
                .Include(c => c.Category)
                .ToList();
            return View(courses);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var course = _dbConText.Courses.Single(c => c.Id == id && c.LecturerId == userId);
            var viewModel = new CourseViewModel
            {
                Categories = _dbConText.Categories.ToList(),
                Date = course.DateTime.ToString("dd/M/yyyy"),
                Time = course.DateTime.ToString("HH:mm"),
                Category = course.CategoryId,
                Place = course.Place,
                Heading = "Edit Course",
                Id = course.Id
            };
            return View("Create", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CourseViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Categories = _dbConText.Categories.ToList();
                return View("Create", viewModel);
            }
            var userId = User.Identity.GetUserId();
            var course = _dbConText.Courses.Single(c => c.Id == viewModel.Id && c.LecturerId == userId);

            course.Place = viewModel.Place;
            course.DateTime = viewModel.GetDateTime();
            course.CategoryId = viewModel.Category;

            _dbConText.SaveChanges();
            return RedirectToAction("Mine", "Courses");
        }



        public void UnAtten(int id)
        {
            var userID = User.Identity.GetUserId();
            var row = _dbConText.Attendances.Where(p => p.AttendeeId.Equals(userID) && p.CourseId == id).SingleOrDefault();
            _dbConText.Attendances.Remove(row);
            _dbConText.SaveChanges();
        }


        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();
            var followings = _dbConText.Followings
                .Where(a => a.FollowerId == userId)
                .Select(a => a.Followee)
                .ToList();

            var viewModel = new FollowingViewModels
            {
                Followings = followings,
                ShowAction = User.Identity.IsAuthenticated
            };

            return View(viewModel);
        }

        public void Unfollow(string followeeId)
        {
            var userID = User.Identity.GetUserId();
            var folower = _dbConText.Followings.Where(p => p.FolloweeId.Equals(followeeId) && p.FollowerId.Equals(userID)).SingleOrDefault();
            _dbConText.Followings.Remove(folower);
            _dbConText.SaveChanges();
        }
    }
    

}