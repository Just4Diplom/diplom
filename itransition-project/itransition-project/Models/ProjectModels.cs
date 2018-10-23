using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace itransition_project.Models
{
    public class Profile
    {
        public int Id { get; set; }
        [Required]
        public virtual ApplicationUser User { get; set; }
        public string Photo { get; set; }
        public string About { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Medal> Medals { get; set; }
        public virtual ICollection<Comix> Comixes { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public virtual Profile Profile { get; set; }
    }

    public class Medal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public virtual Profile Profile { get; set; }
    }

    public class Rating
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool Condition { get; set; }
    }

    public class Comix
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual int RatingValue { get; set; }
        public virtual AgeRating AgeRating { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }

    public class Page
    {
        public int Id { get; set; }
        public virtual Template Template { get; set; }
        public virtual ICollection<Frame> Frames { get; set; }
    }

    public class Frame
    {
        public int Id { get; set; }
        public string BackgroundImage { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public virtual ICollection<Balloon> Balloons { get; set; }
    }

    public class AgeRating
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual ICollection<Comix> Comixes { get; set; }
    }

    public class Template
    {
        public int Id { get; set; }
        public int PositionsCount { get; set; }
        public string Type { get; set; }
    }

    public class Balloon
    {
        public int Id { get; set; }
        public virtual BalloonType Type { get; set; }
        public string Text { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }

    public class BalloonType
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
    //public partial class ProjectDbContext : DbContext
    //{

    //    public ProjectDbContext() : base("DefaultConnection")
    //    {
    //    }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        //base.OnModelCreating(modelBuilder);
    //        modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
    //        modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
    //        modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

    //        modelBuilder.Entity<ApplicationUser>()
    //            .HasRequired(s => s.Profile)
    //            .WithRequiredPrincipal(s => s.User);

    //        modelBuilder.Entity<Profile>()
    //            .HasRequired(s => s.User)
    //            .WithRequiredDependent(s => s.Profile);
    //    }

    //    public DbSet<Profile> Profiles { get; set; }
    //    public DbSet<Comment> Comments { get; set; }
    //    public DbSet<Medal> Medals { get; set; }
    //    public DbSet<Rating> Ratings { get; set; }
    //    public DbSet<Comix> Comixes { get; set; }
    //    public DbSet<Page> Pages { get; set; }
    //    public DbSet<Frame> Frames { get; set; }
    //    public DbSet<AgeRating> AgeRatings { get; set; }
    //    public DbSet<Tag> Tags { get; set; }
    //    public DbSet<Template> Templates { get; set; }
    //    public DbSet<Balloon> Balloons { get; set; }
    //    public DbSet<BalloonType> BallonTypes { get; set; }
    //}
}