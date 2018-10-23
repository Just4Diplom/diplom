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

namespace itransition_project.Controllers
{
    public class ComixController : Controller
    {
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

            Comix c = new Comix() {
                Author = currentAppUser,
                CreationTime = DateTime.Now,
                Name = comix.Name,
                Pages = new List<Page>(),
                Tags = new List<Tag>()
            };

            var tags = new List<string>();

            var tagSet = db.Tags.ToList();

            if (comix.Tags.Count != 0)
                foreach (var tag in comix.Tags)
                {
                    var currentTag = tagSet.FirstOrDefault(t => t.Text.Equals(tag.text));
                    if (currentTag == null)
                    {
                        c.Tags.Add(new Tag { Text = tag.text});
                    }
                    else
                    {
                        c.Tags.Add(currentTag);
                    }
                }

            foreach (var page in comix.Pages)
            {
                Page p = new Page()
                {
                    Template = db.Templates.First(x => x.Type == page.Template),
                    Frames = new List<Frame>()
                };

                foreach (var image in page.FrameImages)
                {
                    var account = new Account(
                    "da40pd4iw",
                    "878111261769614",
                    "d_UzO32EJIqhtFnshPcdgalOFeg");

                    var cloudinary = new Cloudinary(account);

                    string bg = image.BackgroundImage;

                    if (bg.Substring(0, 4) == "data")
                    { 
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(bg)
                        };

                        var uploadResult = cloudinary.Upload(uploadParams);

                        bg = "url(\"" + uploadResult.SecureUri.ToString() + "\")";
                    }

                    Frame f = new Frame()
                    {
                        BackgroundImage = bg,
                        Top = image.Top,
                        Left = image.Left,
                        Width = image.Width,
                        Height = image.Height,
                        Balloons = new List<Balloon>()
                    };

                    if (image.Balloons != null)
                    foreach (var balloon in image.Balloons)
                    {
                        Balloon b = new Balloon()
                        {
                            Text = balloon.Text,
                            Top = balloon.Top,
                            Left = balloon.Left,
                            Width = balloon.Width,
                            Height = balloon.Height
                        };
                        f.Balloons.Add(b);
                    }
                    p.Frames.Add(f);
                }
                c.Pages.Add(p);
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
                Pages = new List<JsonPagesViewModel>()
            };

            foreach (var tag in comix.Tags)
            {
                comixViewModel.Tags.Add(new TagText { text = tag.Text });
            }

            foreach (var page in comix.Pages)
            {
                var p = new JsonPagesViewModel()
                {
                    Template = page.Template.Type,
                    FrameImages = new List<JsonFrameImageViewModel>()
                };

                foreach (var frame in page.Frames)
                {
                    var f = new JsonFrameImageViewModel()
                    {
                        BackgroundImage = frame.BackgroundImage,
                        Top = frame.Top,
                        Left = frame.Left,
                        Width = frame.Width,
                        Height = frame.Height,
                        Balloons = new List<JsonBalloonsViewModel>()
                    };

                    if (frame.Balloons != null)
                        foreach (var balloon in frame.Balloons)
                        {
                            var b = new JsonBalloonsViewModel()
                            {
                                Text = balloon.Text,
                                Top = balloon.Top,
                                Left = balloon.Left,
                                Width = balloon.Width,
                                Height = balloon.Height
                            };
                            f.Balloons.Add(b);
                        }
                    p.FrameImages.Add(f);
                }
                comixViewModel.Pages.Add(p);
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
            comix.Ratings.Add(new Rating {Condition = isPositive, User = user});
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