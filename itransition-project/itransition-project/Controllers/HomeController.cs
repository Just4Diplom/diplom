using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using itransition_project.Filters;
using itransition_project.Lucene;
using itransition_project.Models;

namespace itransition_project.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            List<Comix> model = db.Comixes.OrderByDescending(c => c.CreationTime).ToList();
            return View(model);
        }

        public ActionResult GetTagsForCloud()
        {
            var db = new ApplicationDbContext();
            var lst = db.Tags.Select(tag => new TagUsing
            {
                TagName = tag.Text,
                Uses = tag.Comixes.Count
            }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetTagsForAutocomplete(string id)
        {
            var db = new ApplicationDbContext();
            var lst =
               (from tag in db.Tags
                where tag.Text.StartsWith(id)
                select new TagText { text = tag.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public class TagUsing
        {
            public string TagName { get; set; }
            public int Uses { get; set; }
        }

        public class TagText
        {
            public string text { get; set; }
        }
    }
}