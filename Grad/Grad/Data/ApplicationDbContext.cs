using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Grad.Models;

namespace Grad.Data
{
    public class EditorContext : IdentityDbContext<User>
    {
        public EditorContext(DbContextOptions<EditorContext> options)
            : base(options)
        {
        }

        public DbSet<ArticleRequest> ArticleRequests { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Grad.Models.User> User { get; set; }

        public DbSet<Grad.Models.Status> Status { get; set; }
    }
}
