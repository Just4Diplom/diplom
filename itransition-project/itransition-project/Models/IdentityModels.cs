using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace itransition_project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual Profile Profile { get; set; }
        public string Language { get; set; }
        public string Theme { get; set; }
        public string Name { get; set;}
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<ApplicationUser>()
                .HasRequired(s => s.Profile)
                .WithRequiredPrincipal(s => s.User);

            //modelBuilder.Entity<Profile>()
            //    .HasRequired(s => s.User)
            //    .WithRequiredDependent(s => s.Profile);
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Medal> Medals { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comix> Comixes { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Frame> Frames { get; set; }
        public DbSet<AgeRating> AgeRatings { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Balloon> Balloons { get; set; }
        public DbSet<BalloonType> BallonTypes { get; set; }
    }
}