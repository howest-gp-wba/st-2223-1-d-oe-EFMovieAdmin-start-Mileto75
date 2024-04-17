using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wba.Oefening.RateAMovie.Core.Entities;

namespace Wba.Oefening.RateAMovie.Web.Data
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Actor> Actors { get; set; }

        public DbSet<Director> Directors { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Rating> Ratings { get; set; }
       
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configure the User Entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.FirstName)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.LastName)
                .HasMaxLength(100);

            //Configure the Movie Entity
            modelBuilder.Entity<Movie>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Movie>()
                .Property(u => u.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Movie>()
                .Property(u => u.Title)
                .IsRequired()
                .HasMaxLength(200);

            //Configure the Actor Entity
            modelBuilder.Entity<Actor>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Actor>()
                .Property(u => u.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Actor>()
                .Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Actor>()
                .Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            //Configure the Director Entity
            modelBuilder.Entity<Director>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Director>()
                .Property(u => u.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Director>()
                .Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Director>()
                .Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            //Configure the Company Entity
            modelBuilder.Entity<Company>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Company>()
                .Property(u => u.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Company>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(200);


            //Configure the Rating Entity
            modelBuilder.Entity<Rating>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Rating>()
                .Property(u => u.Id)
                .UseIdentityColumn();

            //finally, seed this db
            Seeder.SeedData(modelBuilder);
        }

    }
}
