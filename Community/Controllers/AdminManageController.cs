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
    [Authorize(Roles = "Admin")]
    public class AdminManageController : Controller
    {
        // GET: Admin
        public ActionResult Post()
        {

            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            PostModelVm pvm = new PostModelVm();

            var posts = dbContext.Posts.OrderByDescending(q=> q.created_at).ToList();
            pvm.posts = posts;

            return View(pvm);
        }

        [HttpGet]
        public ActionResult PostFilter(string filterName)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var posts = dbContext.Posts.Where(q => q.Id !=0);

            //if (filterName == "pending")
            //{
            //    posts = posts.Where(q => q.Status == 0);
            //}

            //if (filterName == "approved")
            //{
            //    posts = posts.Where(q => q.Status == 1);
            //}

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
          

        // POST: Admin/Edit/5
        [HttpPost]
        public string updateStatus(FormCollection collection)
        {
            //try
            //{
                int postId = int.Parse(collection.Get("postId"));
                int status = int.Parse(collection.Get("status"));

                var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                PostModelVm pvm = new PostModelVm();

                var post = dbContext.Posts.Where(q => q.Id == postId).SingleOrDefault();
                post.Status = status;
                dbContext.SaveChanges();

                return "Ads is updated sucessfully";

            //}
            //catch
            //{
                //return "Something went wrong";

            //}
        }

    }
}
