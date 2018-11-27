using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using itransition_project.Models;
using Microsoft.AspNet.Identity.Owin;
using AutoMapper;
using Microsoft.AspNet.Identity;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using itransition_project.Lucene;
using System.IO;
using itransition_project.Filters;

namespace itransition_project.Controllers
{
    [Culture]
    public class ComixController : Controller
    {

        public class FaceGroupModel
        {
            public string id { get; set; }
            public string title { get; set; }
        }



        public class FaceModel
        {
            public string file_name { get; set; }
            public string category_id { get; set; }
        }

        public JsonResult faces_groups(string _)
        {
            string path = HttpContext.Server.MapPath(@"/Content/StaticImages/");
            string[] subfolders = System.IO.Directory.GetDirectories(path);
            List<FaceGroupModel> faceGroupModels = new List<FaceGroupModel>();
            for (int i = 0; i < subfolders.Length; i++)
            {
                subfolders[i] = subfolders[i].Split('\\').Last();
                faceGroupModels.Add(new FaceGroupModel() { id = subfolders[i], title = subfolders[i]});
            }
            return Json(faceGroupModels, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult faces(string gid, string _)
        {
            string path = Server.MapPath(@"/Content/StaticImages/"+ gid);
            List<string> files = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories).ToList();
            List<FaceModel> faceModels = new List<FaceModel>();

            for (int i = 0; i < files.Count; i++)
            {
                files[i] = files[i].Split('\\').Last();
                faceModels.Add(new FaceModel() { file_name = files[i], category_id = gid });
            }
            return Json(faceModels, JsonRequestBehavior.AllowGet); ;
            //return @"[{ ""file_name"":""nrIUH3r4.png"",""category_id"":""5""}]";
        }

        public ActionResult images(string img)
        {
            var dir = Server.MapPath(@"/Content/StaticImages/");
            var files = Directory.GetFiles(dir, "*.png", SearchOption.AllDirectories).ToList();
            foreach (var file in files)
            {
                if (file.Split('\\').Last() == img)
                {
                    return File(System.IO.File.ReadAllBytes(file), @"image/png");
                }
            }
            return this.HttpNotFound();
        }
        const string ExpectedImagePrefix = "data:image/jpeg;base64,";

        public class Image
        {
            public string image { get; set; }
        }
        [HttpPost]
        public JsonResult image_from_file(Image qwe)
        {
            var file = Request.Files[0]; //Uploaded file
            if (Request.Files.Count > 0)
            {

                if (file != null && file.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                    {
                        var a = new Image() { image = "" };
                        var q = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                        var fileName = GetMimeType(file.FileName);
                        a.image = "data:" + fileName + ";base64," + Convert.ToBase64String(q);
                        var result = Json(a, JsonRequestBehavior.AllowGet);
                        result.ContentType = "text/html; charset=UTF-8";
                        return result;
                    }
                }
            }

            return null;
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        // GET: Page
        public ActionResult Index(int id)
        {
            var db = new ApplicationDbContext();
            var comixModel = db.Comixes.FirstOrDefault(x => x.Id.Equals(id));
            //var comixViewModel = Mapper.Map<Comix, ComixViewModel>(comixModel);
            //return View(comixViewModel);
            return View(comixModel);
        }

        [Authorize]
        public ActionResult AddComix()
        {
            return View();
        }

        public ActionResult ComixPage()
        {
            return View();
        }

        public ActionResult NewComix()
        {
            return View();
        }

        public ActionResult ViewComix(int id)
        {
            return View(MakeModel(id));
        }

        public ActionResult EditComix()
        {
            return View();
        }

        public ActionResult ReceiveComix(JsonComixViewModel comix)
        {
            var db = new ApplicationDbContext();

            var currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentAppUser = db.Users.First(x => x.Id == currentUserId);

            Comix c = new Comix()
            {
                Author = currentAppUser,
                CreationTime = DateTime.Now,
                Name = comix.Name,
                Image = comix.Image,
                Tags = new List<Tag>()
            };

            var tags = new List<string>();

            var tagSet = db.Tags.ToList();
            if (comix.Tags != null)
            {
                if (comix.Tags.Count != 0)
                    foreach (var tag in comix.Tags)
                    {
                        var currentTag = tagSet.FirstOrDefault(t => t.Text.Equals(tag));
                        if (currentTag == null)
                        {
                            c.Tags.Add(new Tag { Text = tag });
                        }
                        else
                        {
                            c.Tags.Add(currentTag);
                        }
                    }
            }
            currentAppUser.Profile.Comixes.Add(c);
            db.SaveChanges();
            LuceneEntryModel.AddUpdateLuceneIndex(currentAppUser.Profile.Comixes.Last());
            return Json(Url.Action("UserInfo", "User", new { id = User.Identity.GetUserName() }));
        }


        public JsonResult SendComix(int id)
        {
            return Json(MakeModel(id), JsonRequestBehavior.AllowGet);
        }

        private JsonReturnComixViewModel MakeModel(int id)
        {
            var db = new ApplicationDbContext();
            var comix = db.Comixes.First(x => x.Id == id);

            AuthorViewModel author = new AuthorViewModel()
            {
                Id = comix.Author.Id,
                UserName = comix.Author.UserName
            };

            JsonReturnComixViewModel comixViewModel = new JsonReturnComixViewModel
            {
                Id = comix.Id,
                Author = author,
                CreationTime = comix.CreationTime,
                Name = comix.Name,
                Tags = new List<TagText>(),
                Image = comix.Image
            };

            foreach (var tag in comix.Tags)
            {
                comixViewModel.Tags.Add(new TagText { text = tag.Text });
            }

            return comixViewModel;
        }

        [HttpPost]
        public ActionResult GetRate(int Id)
        {
            var db = new ApplicationDbContext();
            var edb = db.Comixes.First(x => x.Id == Id);
            int rating = edb.Ratings.Sum(userRate => userRate.Condition ? 1 : -1);
            return Json(rating, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetUserRate(int Id)
        {
            var db = new ApplicationDbContext();
            if (User.Identity.IsAuthenticated)
            {
                var comix = db.Comixes.First(x => x.Id == Id);
                var currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                var user = db.Users.First(x => x.Id == currentUserId);
                var userRate = comix.Ratings.FirstOrDefault(x => x.User.Id == user.Id);
                return Json(userRate?.Condition, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddRate(int Id, bool isPositive)
        {
            var db = new ApplicationDbContext();
            var comix = db.Comixes.First(x => x.Id == Id);
            var currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.First(x => x.Id == currentUserId);

            foreach (var userRate in comix.Ratings)
            {
                if (userRate.User.Id == user.Id)
                {
                    if (userRate.Condition != isPositive)
                    {
                        userRate.Condition = !userRate.Condition;
                        var a = db.Comixes.First(x => x.Id == comix.Id);
                        a = comix;
                        db.SaveChanges();
                        return null;
                    }
                    else
                    {
                        comix.Ratings.Remove(userRate);
                        db.Ratings.Remove(userRate);
                        db.SaveChanges();
                        var b = db.Comixes.First(x => x.Id == comix.Id);
                        b = comix;
                        db.SaveChanges();
                        return null;
                    }
                }
            }
            comix.Ratings.Add(new Rating { Condition = isPositive, User = user });
            var c = db.Comixes.First(x => x.Id == comix.Id);
            var edb = db.Comixes.First(x => x.Id == Id);
            int rating = edb.Ratings.Sum(userRate => userRate.Condition ? 1 : -1);
            c = comix;
            c.RatingValue = rating;
            db.SaveChanges();
            return null;
        }

        public ActionResult GetTagsForAutocomplete(string id)
        {
            var db = new ApplicationDbContext();
            var lst =
               (from tag in db.Tags.ToList()
                where tag.Text.StartsWith(id)
                select new TagText { text = tag.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}