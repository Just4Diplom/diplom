using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using itransition_project.Filters;
using itransition_project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace itransition_project.Controllers
{
    public class UserController : Controller
    {
        public static string UserPageId;
        // GET: User
        [HttpGet]
        [Culture]
        public ActionResult UserInfo(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            if (db.Users.Any(o => o.UserName == id))
            {
                UserPageId = id;
                var model = new UserInfoViewModel
                {
                    Comments = db.Comments.Where(o => o.Profile.User.UserName == id).OrderByDescending(x => x.Time).ToList(),
                    Profile = db.Users.First(o => o.UserName == id),
                    Medals = db.Medals.Where(o => o.Profile.User.UserName == id).ToList(),
                    Comixes = db.Comixes.Where(o => o.Author.UserName == id).ToList()
                };
                return View(model);
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult UserInfo(string text, string userId)
        {
            var dbContext = new ApplicationDbContext();
            var currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentAppUser = dbContext.Users.First(x => x.Id == currentUserId);
            var user = dbContext.Users.First(x => x.Id == userId);
            var comment = new Comment
            {
                Time = DateTime.Now,
                Author = currentAppUser,
                Text = text
            };
            user.Profile.Comments.Add(comment);
            if (!dbContext.Comments.Any(x => x.Author.Id == currentUserId))
            {
                currentAppUser.Profile.Medals.Add(new Medal
                {
                    Image = "http://res.cloudinary.com/da40pd4iw/image/upload/v1462468044/medal_comments_vquan0.png",
                    Name = "First Comment",
                    Profile = currentAppUser.Profile
                });
            }
            if (dbContext.Comments.Count(x => x.Author.Id == currentUserId) == 4)
            {
                currentAppUser.Profile.Medals.Add(new Medal
                {
                    Image = "http://res.cloudinary.com/da40pd4iw/image/upload/v1462468044/medal_ban_ipfftk.png",
                    Name = "5 comments",
                    Profile = currentAppUser.Profile
                });
                dbContext.Medals.Remove(currentAppUser.Profile.Medals.First(x => x.Name == "First Comment"));
            }
            dbContext.SaveChanges();
            return RedirectToAction("UserInfo", "User");
        }

        [HttpGet]
        [Culture]
        public ActionResult EditDetails()
        {
            ApplicationUser appUser = System.Web.HttpContext.Current.GetOwinContext().
                GetUserManager<ApplicationUserManager>().
                FindById(System.Web.HttpContext.
                Current.User.Identity.GetUserId());
            return View(appUser.Profile);
        }

        [HttpPost]
        public ActionResult RefreshInfo(string id, string about, string theme, string lang)
        {
            var dbContext = new ApplicationDbContext();
            ApplicationUser applicationUser = dbContext.Users.First(x => x.UserName == id);
            applicationUser.Theme = theme;
            applicationUser.Profile.About = about;
            applicationUser.Language = lang; 
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;   // если куки уже установлено, то обновляем значение
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            dbContext.SaveChanges();
            return RedirectToAction("UserInfo", "User", id);
        }

        [HttpPost]
        public ActionResult DelComment(int data)
        {
            var dbContext = new ApplicationDbContext();
            Comment comment = dbContext.Comments.First(x => x.Id == data);
            dbContext.Comments.Remove(comment);
            dbContext.SaveChanges();
            return RedirectToAction("UserInfo", "User");
        }

        [HttpPost]
        public ActionResult DelComix(int data)
        {
            var dbContext = new ApplicationDbContext();
            Comix comix = dbContext.Comixes.First(x => x.Id == data);

            foreach(var tag in comix.Tags.ToList())
            {
                comix.Tags.Remove(tag);
            }

            foreach(var rating in comix.Ratings.ToList())
            {
                dbContext.Ratings.Remove(rating);
            }
            
            foreach(var page in comix.Pages.ToList())
            {
                foreach(var frame in page.Frames.ToList())
                {
                    foreach(var balloon in frame.Balloons.ToList())
                    {
                        frame.Balloons.Remove(balloon);
                    }
                    page.Frames.Remove(frame);
                }
                comix.Pages.Remove(page);
            }

            comix.Author.Profile.Comixes.Remove(comix);
            dbContext.Comixes.Remove(comix);
            dbContext.SaveChanges();
            return Json(Url.Action("UserInfo", "User", new { id = User.Identity.GetUserName() }));
        }

        [HttpPost]
        public JsonResult UpdateImage(string data)
        {
            var dbContext = new ApplicationDbContext();
            var account = new Account(
                "da40pd4iw",
            "878111261769614",
            "d_UzO32EJIqhtFnshPcdgalOFeg");

            var cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(data)
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            var user = System.Web.HttpContext.Current.GetOwinContext().
                GetUserManager<ApplicationUserManager>().
                FindById(System.Web.HttpContext.
                Current.User.Identity.GetUserId());
            if (user.Profile.Photo == "http://res.cloudinary.com/da40pd4iw/image/upload/v1460917537/%D0%9F%D0%B8%D0%B2%D0%BE_y9a59r.jpg")
            {
                user.Profile.Medals.Add(new Medal
                {
                    Image = "http://res.cloudinary.com/da40pd4iw/image/upload/v1462468044/medal_profile_t9bltt.png",
                    Name = "Photo add",
                    Profile = user.Profile
                });
            }
            user.Profile.Photo = uploadResult.SecureUri.ToString();
            
            
            System.Web.HttpContext.Current.GetOwinContext().
                GetUserManager<ApplicationUserManager>().Update(user);
            dbContext.SaveChanges();
            return new JsonResult();
        }
    }
}