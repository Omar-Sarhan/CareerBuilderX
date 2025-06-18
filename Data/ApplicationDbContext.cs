using CareerBuilderX.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareerBuilderX.Data
{
    public class ApplicationDbContext : IdentityDbContext<Person>
    {
        public DbSet<Admin> Admins { get; set; }

        public DbSet<EndUser> EndUsers { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Resume> Resumes { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<ServicePortfolio> ServicePortfolios { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Resume - EndUser (One-to-Many)
            modelBuilder.Entity<Resume>()
                .HasOne(r => r.EndUser)
                .WithMany(e => e.Resumes)
                .HasForeignKey(r => r.EndUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.EndUser)
                .WithMany(e => e.Portfolios)
                .HasForeignKey(p => p.EndUserId)
                .OnDelete(DeleteBehavior.Cascade);


            // Configure Certification
            modelBuilder.Entity<Certification>()
                .HasOne(c => c.Resume)
                .WithMany(r => r.Certifications)
                .HasForeignKey(c => c.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Education
            modelBuilder.Entity<Education>()
                .HasOne(e => e.Resume)
                .WithMany(r => r.Educations)
                .HasForeignKey(e => e.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Experience
            modelBuilder.Entity<Experience>()
                .HasOne(e => e.Resume)
                .WithMany(r => r.Experiences)
                .HasForeignKey(e => e.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Language
            modelBuilder.Entity<Language>()
                .HasOne(l => l.Resume)
                .WithMany(r => r.Languages)
                .HasForeignKey(l => l.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Skill
            modelBuilder.Entity<Skill>()
                .HasOne(s => s.Resume)
                .WithMany(r => r.Skills)
                .HasForeignKey(s => s.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Skill>()
                .HasOne(s => s.Portfolio)
                .WithMany(r => r.Skills)
                .HasForeignKey(s => s.PortfolioId);

            // Configure Project - Portfolio
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Portfolio)
                .WithMany(pf => pf.Projects)
                .HasForeignKey(p => p.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServicePortfolio>()
    .HasKey(sp => new { sp.PortfolioId, sp.ServiceId });

            modelBuilder.Entity<ServicePortfolio>()
                .HasOne(sp => sp.Portfolio)
                .WithMany(p => p.servicePortfolios)
                .HasForeignKey(sp => sp.PortfolioId);

            modelBuilder.Entity<ServicePortfolio>()
                .HasOne(sp => sp.Service)
                .WithMany(s => s.servicePortfolios)
                .HasForeignKey(sp => sp.ServiceId);



            // Optional: Configure nullable navigation to Service inside Project
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Service)
                .WithMany(p => p.Projects)
                .HasForeignKey(p => p.ServiceId);
                

            modelBuilder.Entity<Resume>()
                .Property(t => t.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Portfolio>()
                .Property(t => t.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Experience>()
                .Property(t => t.IsCurrent)
                .HasDefaultValue(false);
            modelBuilder.Entity<Service>()
                .Property(t => t.IsDeleted)
                .HasDefaultValue(false);


        }

    }
}