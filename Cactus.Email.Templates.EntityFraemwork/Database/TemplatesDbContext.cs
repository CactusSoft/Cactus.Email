using Cactus.Email.Templates.EntityFraemwork.Managers;
using Microsoft.EntityFrameworkCore;

namespace Cactus.Email.Templates.EntityFraemwork.Database
{
    public class TemplatesDbContext : DbContext
    {
        public TemplatesDbContext(DbContextOptions<TemplatesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Template

            modelBuilder.Entity<Template>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Template>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<Template>()
                .Property(x => x.Language)
                .IsRequired();

            modelBuilder.Entity<Template>()
                .HasIndex(x => new { x.Name, x.Language })
                .IsUnique();

            modelBuilder.Entity<Template>()
                .Property(x => x.SubjectTemplate)
                .IsRequired(false);

            modelBuilder.Entity<Template>()
                .Property(x => x.HtmlBodyTemplate)
                .IsRequired(false);

            modelBuilder.Entity<Template>()
                .Property(x => x.PlainBody)
                .IsRequired(false);

            modelBuilder.Entity<Template>()
                .Property(x => x.PlainBodyEncoding)
                .IsRequired(false);

            modelBuilder.Entity<Template>()
                .Property(x => x.HtmlBodyEncoding)
                .IsRequired(false);

            modelBuilder.Entity<Template>()
                .Property(x => x.CreatedDateTime)
                .IsRequired();
            #endregion
        }

        public DbSet<Template> Templates { get; set; }
    }
}
