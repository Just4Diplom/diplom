using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using itransition_project.Lucene;
using Lucene.Net.Search;
using itransition_project.Models;

namespace itransition_project.Controllers
{
    public class SearchController: Controller
    {
        public ActionResult Index(string id)
        {
            List<Comix> findComixes = LuceneEntryModel.Search(id, null).ToList();
            return View(findComixes);
        }

        public ActionResult SearchByTag(string id)
        {
            List<Comix> findComixes = LuceneEntryModel.SearchByTag(id, null).ToList();
            return View(findComixes);
        }
    }
}