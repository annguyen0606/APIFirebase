using FirebaseASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirebaseASP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            AuthorANC authorANC = new AuthorANC();
            authorANC.Name = "An Nguyễn";
            authorANC.Address = "Minh Khai - Hoài Đức - Hà Tây";
            authorANC.BirthDay = "06/06/1996";
            authorANC.University = "VNU University of Engineering and Technology";
            authorANC.Phone = "0356435101";
            ViewBag.Message = "Your application description page.";

            return View(authorANC);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}