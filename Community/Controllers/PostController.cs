using Community.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;

namespace Community.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var countries = dbContext.Countries.ToList();
            var states = dbContext.States.ToList();

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

            return View(postvm);
        }

        // GET: Post/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public String GetStates(int? countryId)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var states = dbContext.States.Where(item => item.CountryId == countryId).ToList();

            string statesHtml = "<option value='0'>--Select Region--</option>";

            foreach (var item in states)
            {
                statesHtml += "<option value="+item.Id+" >"+item.Name+"</option>";
            }

            return statesHtml;
        }

        [HttpGet]
        public String GetCities(int? regionId)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var cities = dbContext.Cities.Where(item => item.StateId == regionId).ToList();

            string citiesHtml = "<option value='0'>--Select City--</option>";

            foreach (var item in cities)
            {
                citiesHtml += "<option value=" + item.Id + " >" + item.Name + "</option>";
            }

            return citiesHtml;
        }

        // POST: Post/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Post/Save
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Save( FormCollection collection)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            int userId = 1; /*User.Identity.GetUserId<int>();*/
            int lastInsertedPostId = 0;

            Post pm = new Post();
            pm.UserId = userId;
            pm.AdType = Convert.ToInt32(collection.Get("ad_type"));
            pm.AdvertiserType = Convert.ToInt32(collection.Get("ads_type"));
            pm.ExpireTime = DateTime.Parse(collection.Get("expireTime"));
            pm.CountryId = Convert.ToInt32(collection.Get("country"));
            pm.RegionId = Convert.ToInt32(collection.Get("region"));
            pm.CityId = Convert.ToInt32(collection.Get("city"));
            pm.CategoryId = Convert.ToInt32(collection.Get("parentCateogry"));
            pm.Title = collection.Get("title");
            pm.Price = Convert.ToDouble(collection.Get("price"));
            pm.Description = collection.Get("description");
            pm.PhoneNumber = collection.Get("phoneNumber");
            pm.Address = collection.Get("streeAddress");
            pm.PostalCode = collection.Get("postalCode");
            pm.VideoUrl = collection.Get("videoUrl");
            pm.Status = 0;      //0: pending, 1: success
            dbContext.Posts.Add(pm);
            dbContext.SaveChanges();
            lastInsertedPostId = pm.Id;

            this.handleFile(Request.Files, lastInsertedPostId);

            //this.TagManagement(collection("Tags"));   // Add Attachments

            return RedirectToAction("Index");
        }

        private void handleFile(HttpFileCollectionBase fileCollection, int postId)
        {
            if (fileCollection.Count > 0)
            {
                var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                Attachment attachment = new Attachment();

                for (int i = 0; i < fileCollection.Count; i++)
                {
                Random r = new Random();
                string path = "-1";
                int random = r.Next();
                
                    string extension = Path.GetExtension(fileCollection[i].FileName);
                    if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                    {
                        try
                        {
                            path = Path.Combine(Server.MapPath("~/assets/uploads/"), random + Path.GetFileName(fileCollection[i].FileName));
                            fileCollection[i].SaveAs(path);
                            path = "~/assets/uploads/" + random + Path.GetFileName(fileCollection[i].FileName);
                            
                            //Store images by postId

                            attachment.PostId   = postId;
                            attachment.FileName = fileCollection[i].FileName;
                            attachment.FilePath = path;
                            attachment.HashName = random + Path.GetFileName(fileCollection[i].FileName);
                            dbContext.Attachments.Add(attachment);
                            dbContext.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            path = "-1";
                        }
                    }
                    else
                    {
                        Response.Write("<script>only jpg and png</script>");
                    }
                }
            }
        }

        // POST: Post/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
        
            return RedirectToAction("Index"); 
        }
    }
}
