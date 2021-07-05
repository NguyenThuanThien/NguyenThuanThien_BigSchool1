using NguyenThuanThien_BigSchool.Models;
using NguyenThuanThien_BigSchool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public ActionResult Create()
        {
            var ViewModel = new CourseViewModel
            {
                Categories = _dbConText.Categories.ToList()
            };
            return View(ViewModel);
        }
    }
}