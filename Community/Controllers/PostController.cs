using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;
using Community.Models;
using System.Net.Mail;

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

            ViewBag.tags = this.BeautifyTags(tags);

            PostModelVm postvm = new PostModelVm();
            postvm.categories = categories;
            postvm.countries = countries;
            postvm.states = states;

            return View(postvm);
        }

        public string BeautifyTags(List<Tag> tags)
        {
            string[] newTags = new string[tags.Count()];
            var i = 0;
            foreach(var item in tags)
            {
                newTags.SetValue(item.Name, i);
                i++;
            }

            return string.Join(",",  newTags);
        }

        // GET: Post/Details/5
        public ActionResult Details(int id)
        {

            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var post = dbContext.Posts.Where(q => q.Status == 1 && q.Id == id).SingleOrDefault();
            var country = dbContext.Countries.Where(q => q.Id == post.CountryId).SingleOrDefault();
            var refPost = dbContext.Posts.Where(q => q.Status == 1 && q.Id !=id).ToList();
            PostModelVm pvm = new PostModelVm();

            pvm.post  = post;
            pvm.posts = refPost;
            pvm.countryName = country.Name;
            return View(pvm);
        }

        [HttpGet]
        public string GetStates(int id)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var states = dbContext.States.Where(item => item.CountryId == id).ToList();

            string statesHtml = "<option value='0'>--Select Region--</option>";

            foreach (var item in states)
            {
                statesHtml += "<option value="+item.Id+" >"+item.Name+"</option>";
            }

            return statesHtml;
        }

        [HttpPost, ValidateInput(false)]

        public string SendMail(FormCollection collection)
        {
            string to = collection.Get("toEmail");                  //To address    
            string from = collection.Get("fromEmail");              //From address    

            MailMessage message = new MailMessage(from, to);

            string mailbody = collection.Get("mailbody");
            message.Subject = collection.Get("mailSubject");
            message.Body = mailbody;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("yourmail id", "Password");

            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;

            try
            {
                client.Send(message);
                return "The mail is sent successfully!";
            }

            catch (Exception ex)
            {
                return "Something went wrong in sending mail!";
            }

        }

        [HttpGet]
        public string GetCities(int id)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var cities = dbContext.Cities.Where(item => item.StateId == id).ToList();

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
            string userId = HttpContext.User.Identity.GetUserId();

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
            pm.created_at = DateTime.UtcNow;
            pm.Status = 0;      //0: pending, 1: success
            dbContext.Posts.Add(pm);
            dbContext.SaveChanges();

            this.handleFile(Request.Files, pm.Id);
            this.handleTag(collection.Get("Tags"), pm.Id);

            return RedirectToAction("Result");
        }

        public ActionResult Result()
        {
            return View();
        }

        public string Remove(int id)
        {
            try
            {
                var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var attaches = dbContext.Attachments.Where(q => q.PostId == id).ToList();

                Post post = dbContext.Posts.Where(q => q.Id == id).SingleOrDefault();
                dbContext.Posts.Remove(post);
                dbContext.SaveChanges();

                foreach(var item in attaches)
                {
                    string strPhysicalFolder = Server.MapPath("/assets/uploads/");
                    string strFileFullPath   = strPhysicalFolder + item.HashName;

                    FileInfo file = new FileInfo(strFileFullPath);

                    if (file.Exists)        //check file exsit or not  
                    {
                        file.Delete();
                    }
                }
                return "Your Ads is removed successfully";
            }
            catch
            {
                return "Something went wrong";
            }
        }

        private void handleTag(string tags, int postId)
        {
            string[] items = tags.Split(',');
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            dbContext.Database.ExecuteSqlCommand("DELETE FROM TagPosts where Post_Id = " + postId + "");

            if (tags != null && tags !="")
            {


                foreach (var item in items)
                {
                    Tag tagDB = new Tag();
                    var isExistTag = dbContext.Tags.Where(q => q.Name == item).SingleOrDefault();
                    int tag_id = 0;

                    if (isExistTag == null)
                    {
                        tagDB.Name = item;
                        dbContext.Tags.Add(tagDB);
                        dbContext.SaveChanges();
                    }

                    int i = isExistTag == null ? tagDB.Id : isExistTag.Id;

                    isExistTag = dbContext.Tags.Where(q => q.Name == item).SingleOrDefault();

                    tag_id = isExistTag == null ? tagDB.Id : isExistTag.Id;
                    dbContext.Database.ExecuteSqlCommand("INSERT INTO TagPosts (Tag_id, Post_id) VALUES(" + tag_id + ", " + postId + ")");
                    dbContext.SaveChanges();
                }
            }
        }
        public ActionResult Search(string query, FormCollection collection)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            PostModelVm pvm = new PostModelVm();

            var  posts = from e in dbContext.Posts
                                   where e.Title.Contains(query) || e.Description.Contains(query)
                         select e;

            pvm.posts = posts.ToList();

            return View(pvm);
        }
        private void handleFile(HttpFileCollectionBase fileCollection, int postId)
        {
            if (fileCollection.Count > 0)
            {
                var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                Models.Attachment attachment = new Models.Attachment();

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
                            path = "../../assets/uploads/" + random + Path.GetFileName(fileCollection[i].FileName);
                            
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
