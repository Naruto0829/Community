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
using Microsoft.Ajax.Utilities;

namespace Community.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            this.SetLocation();
            this.HandleExpireTime();

            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            
            var countries = dbContext.Countries.ToList();
            var states    = dbContext.States.ToList();
            var tags      = dbContext.Tags.ToList();
            
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
            postvm.tags        = tags;
            postvm.posts       = this.GetPostsByLoation();
            postvm.countryName = Session["countryName"].ToString();
            postvm.regionName  = Session["regionName"].ToString();
            postvm.cityName    = Session["cityName"].ToString();

            return View(postvm);
        }

        public void SetLocation()
        {
            bool isSetedSession = Convert.ToString(Session["countryName"]) != "" ? true : false;

            if (!isSetedSession)
            {

                var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

                string countryName = RegionInfo.CurrentRegion.DisplayName;
                string regionName = RegionInfo.CurrentRegion.ThreeLetterWindowsRegionName;

                var country = dbContext.Countries.Where(q => q.Name.ToLower().Contains(countryName.ToLower())).FirstOrDefault();
                var region = dbContext.States.Where(q => q.Name.ToLower().Contains(regionName.ToLower())).FirstOrDefault();

                int countryId = country != null ? int.Parse(country.Id.ToString()) : 0;
                int regionId  = region  != null ? int.Parse(region.Id.ToString())  : 0;

                Session["countryName"] = countryName;
                Session["regionName"]  = regionName;
                Session["cityName"]    = "";

                Session["countryId"] = countryId;
                Session["regionId"] = regionId;
                Session["cityId"]   = "0";
            }
        }

        public ActionResult filter(string filterName, int category_id)
        {

            int countryId = int.Parse(Session["countryId"].ToString());
            int regionId  = int.Parse(Session["regionId"].ToString());
            int cityId    = int.Parse(Session["cityId"].ToString());

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

            posts = posts.Where(q => q.CountryId == countryId && q.RegionId == regionId);

            if (cityId > 0)
            {
                posts = posts.Where(q => q.CityId == cityId);
            }


            PostModelVm pvm = new PostModelVm();
            pvm.posts = posts.ToList();

            return PartialView("postComponents", pvm);
        }

        public ActionResult GetAdsByTag(int? id)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            PostModelVm pvm = new PostModelVm();

            if(id == 0)
            {
                pvm.posts = dbContext.Posts.Where(q => q.Status == 1).ToList();
            }
            else
            {
                pvm.posts = dbContext.Tags.Where(q => q.Id == id).FirstOrDefault().Posts.Where(q => q.Status == 1).ToList();
            }

            return PartialView("postComponents", pvm);
        }

        public List<Post> GetPostsByLoation()
        {

            int countryId = int.Parse(Convert.ToString(Session["countryId"]));
            int regionId  = int.Parse(Convert.ToString(Session["regionId"]));
            int cityId    = int.Parse(Convert.ToString(Session["cityId"]));

            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var posts = dbContext.Posts.Where(q => q.Status == 1 && q.CountryId == countryId && q.RegionId == regionId);

            if (cityId > 0)
            {
                posts = posts.Where(q => q.CityId == cityId);
            }

            return posts.OrderByDescending(c => c.created_at).ToList();
        }

        public void HandleExpireTime()
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            string query = "DELETE FROM dbo.Posts WHERE Id IN(SELECT Id FROM dbo.Posts WHERE ExpireTime + 1 < CURRENT_TIMESTAMP)";
            dbContext.Database.ExecuteSqlCommand(query);
        }

        public ActionResult ChangeLocation(FormCollection collection)
        {
            int countryId = int.Parse(collection.Get("country"));
            int regionId  = int.Parse(collection.Get("region"));
            int cityId    = int.Parse(collection.Get("city"));

            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var country = dbContext.Countries.Where(q => q.Id == countryId).FirstOrDefault();
            var region  = dbContext.States.Where(q => q.Id == regionId).FirstOrDefault();
            var city    = dbContext.Cities.Where(q => q.Id == cityId).FirstOrDefault();


            Session["countryName"] = country.Name;
            Session["regionName"] = region.Name;
            Session["cityName"] = city.Name;

            Session["countryId"] = country.Id;
            Session["regionId"]  = region.Id;
            Session["cityId"]    = city.Id;

            return RedirectToAction("index");
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