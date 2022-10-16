using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;
using Community.Models;

namespace Community.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var countries = dbContext.Countries.ToList();
            var states = dbContext.States.ToList();
            var posts = dbContext.Posts.ToList();

            var categories = dbContext.Database.SqlQuery<CategoryVM>(@"WITH RCTE AS 
                            (
                                SELECT * , Id AS TopLevelParent
                                FROM dbo.Categories c where ParentId = 0

                                UNION ALL

                                SELECT c.* , r.TopLevelParent
                                FROM dbo.Categories c
                                INNER JOIN RCTE r ON c.ParentId = r.Id
                            )
                            SELECT 
                              r.Id, 
                              r.Name as Name,
                              r.ParentId,
                              r.TopLevelParent,
                              r.Level

                            FROM RCTE r
                            ORDER BY TopLevelParent;").ToList();

            PostModelVm postvm = new PostModelVm();
            postvm.categories = categories;
            postvm.countries = countries;
            postvm.states = states;
            postvm.posts = posts;

            return View(postvm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}