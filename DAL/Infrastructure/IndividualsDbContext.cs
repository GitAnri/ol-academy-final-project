using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Project.Shared.Model;
using System.Security.Cryptography;
using System.Text;
using Shared.Helpers;

namespace Project.DAL.Infrastructure
{
    public class IndividualsDbContext : DbContext
    {
        public IndividualsDbContext(DbContextOptions<IndividualsDbContext> options)
            : base(options) { }

        public DbSet<Individual> Individuals => Set<Individual>();
        public DbSet<PhoneNumber> PhoneNumbers => Set<PhoneNumber>();
        public DbSet<Relation> Relations => Set<Relation>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhoneNumber>()
                .HasOne(p => p.Individual)
                .WithMany(i => i.PhoneNumbers)
                .HasForeignKey(p => p.IndividualId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Relation>()
                .HasOne(r => r.Individual)
                .WithMany(i => i.Relations)
                .HasForeignKey(r => r.IndividualId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Relation>()
                .HasOne(r => r.RelatedIndividual)
                .WithMany()
                .HasForeignKey(r => r.RelatedIndividualId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Individual>()
                .HasIndex(i => i.PersonalNumber)
                .IsUnique();

            modelBuilder.Entity<PhoneNumber>()
                .HasIndex(p => p.Number)
                .IsUnique();


            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Tbilisi" }
            );
            modelBuilder.Entity<City>().HasData(
                new City { Id = 2, Name = "Kutaisi" }
            );
            modelBuilder.Entity<City>().HasData(
                new City { Id = 3, Name = "Batumi" }
            );
            modelBuilder.Entity<City>().HasData(
                new City { Id = 4, Name = "Gori" }
            );
            modelBuilder.Entity<City>().HasData(
                new City { Id = 5, Name = "Zestaponi" }
            );
            modelBuilder.Entity<City>().HasData(
                new City { Id = 6, Name = "Rustavi" }
            );

            var adminPassword = "Admin123!";
            var (hash, salt) = PasswordHelper.HashPassword(adminPassword);

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = hash,
                Salt = salt,
                Role = "Admin"
            });
        }
    }
}
