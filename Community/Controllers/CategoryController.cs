using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Community.Models;
using Microsoft.AspNet.Identity.Owin;

namespace Community.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index(int? page)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

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

            return View(categories);
        }



        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            int parentId = int.Parse(collection[0]);
            string categoryName = collection[1];
            int editId = int.Parse(collection[2]);

            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            Category parentCateogry = dbContext.Categories.Where(item => item.Id == parentId).SingleOrDefault();

            if (editId != 0)
            {
                Category cat = dbContext.Categories.Where(item => item.Id == editId).SingleOrDefault();
                cat.ParentId = parentId;
                cat.Name = categoryName;
            }
            else
            {
                Category cat = new Category();
                cat.ParentId = parentId;
                cat.Name = categoryName;
                cat.Level = parentId > 0 ? parentCateogry.Level + 1 : 1;
                dbContext.Categories.Add(cat);
            }

            dbContext.SaveChanges();

            return RedirectToAction("Index");

        }


        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            Category singleCategory = dbContext.Categories.Where(x => x.Id == id).SingleOrDefault();

            var queryStr = @"WITH RCTE AS (
                                SELECT * , Id AS TopLevelParent
                                FROM dbo.Categories c where ParentId = " + id + @"

                                UNION ALL

                                SELECT c.* , r.TopLevelParent
                                FROM dbo.Categories c
                                INNER JOIN RCTE r ON c.ParentId = r.Id
                            )
                            SELECT 
                              r.Id, 
                              r.Name as Name,
                              r.TopLevelParent AS ParentId ,
                              r.Level

                            FROM RCTE r
                            ORDER BY ParentID;";

            var childrenCategories = dbContext.Database.SqlQuery<CategoryVM>(queryStr).ToList();

            if (childrenCategories.Count > 0)
            {
                foreach (CategoryVM item in childrenCategories)
                {
                    Category category = dbContext.Categories.Where(x => x.Id == item.Id).SingleOrDefault();
                    dbContext.Categories.Remove(category);
                }
            }

            dbContext.Categories.Remove(singleCategory);
            dbContext.SaveChanges();

            return this.GetCategory();
        }


        public ActionResult GetCategory()
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
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

            return PartialView("table", categories);

        }
    }
}
