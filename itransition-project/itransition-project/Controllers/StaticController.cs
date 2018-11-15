using System.IO;
using System.Web.Mvc;

namespace itransition_project.Controllers
{
    public class StaticController : Controller
    {
        [HttpGet]
        public ActionResult Faces(string group, string name)
        {
            var dir = Server.MapPath("/Content/StaticImages/");
            var path = Path.Combine(dir, group + "/" + name);
            return base.File(path, "image/png");
        }

        [HttpGet]
        public ActionResult FacesGroups(string group, string name)
        {
            return this.Content(@"[{""file_name"":""nrIUH3r4.png"",""category_id"":""5""},{""file_name"":""XoWc6Agz.png"",""category_id"":""5""},{""file_name"":""PenRn4CA.png"",""category_id"":""5""},{""file_name"":""PjqKTGE9.png"",""category_id"":""5""},{""file_name"":""NT1xCRsk.png"",""category_id"":""5""},{""file_name"":""LFX4Looi.png"",""category_id"":""5""},{""file_name"":""Xg4rJfSG.png"",""category_id"":""5""},{""file_name"":""UNch9j7X.png"",""category_id"":""5""},{""file_name"":""B255Jbrv.png"",""category_id"":""5""},{""file_name"":""s7XLn9aZ.png"",""category_id"":""5""},{""file_name"":""J37gNX5r.png"",""category_id"":""5""},{""file_name"":""Iqu8SOCW.png"",""category_id"":""5""}]", "application/json");
        }
    }
}
