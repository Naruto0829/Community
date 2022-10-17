using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;
using Community.Models;
using System.Globalization;

namespace Community.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            string countryName = RegionInfo.CurrentRegion.DisplayName;
            string regionName  = RegionInfo.CurrentRegion.ThreeLetterWindowsRegionName;

            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            
            var countries = dbContext.Countries.ToList();
            var states    = dbContext.States.ToList();

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

            postvm.categories  = categories;
            postvm.countries   = countries;
            postvm.states      = states;
            postvm.posts       = this.Search(countryName, regionName);
            postvm.countryName = countryName;
            postvm.regionName  = regionName;

            return View(postvm);
        }

        public ActionResult filter(string filterName, int category_id)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var posts = dbContext.Posts.Where(q => q.Status == 1);

            if (category_id != 0)
            {
                posts = posts.Where(q => q.CategoryId == category_id);
            }

            if (filterName == "newest")
            {
                posts = posts.OrderByDescending(q => q.created_at);
            }
            else if (filterName == "oldest")
            {
                posts = posts.OrderBy(q => q.created_at);
            }
            else if (filterName == "asc")
            {
                posts = posts.OrderBy(q => q.Title);
            }
            else if (filterName == "desc")
            {
                posts = posts.OrderByDescending(q => q.Title);
            }

            PostModelVm pvm = new PostModelVm();
            pvm.posts = posts.ToList();

            return PartialView("postComponents", pvm);
        }

        public List<Post> Search(string countryName, string regionName)
        {
            //string sCountry = Session["country"].ToString();
            //string sRegion  = Session["region"].ToString();
            //string sCity    = Session["city"].ToString();

            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            //var country = dbContext.Countries.Where(q => q.Name == countryName).SingleOrDefault();
            //var region  = dbContext.States.Where(q => q.CountryId == country.Id && q.Name == regionName).SingleOrDefault();
            //var city    = dbContext.Cities.Where(q => q.StateId == region.Id && )
            var posts   = dbContext.Posts.Where(q => q.Status == 1).OrderByDescending(c => c.created_at).ToList();

            return posts;
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