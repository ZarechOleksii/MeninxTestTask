using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Configuration;

namespace DAL
{
    public class LibraryDBContext : DbContext
    {
        public LibraryDBContext() : base() { }

        public LibraryDBContext(DbContextOptions<LibraryDBContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DBLocalConnection"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasIndex(q => q.ISBN)
                .IsUnique();

            Guid categoryGuid = Guid.Parse("1f4494ff-8bb8-4967-9350-c9c4a8d7d697");

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = categoryGuid,
                    Name = "Horror",
                    Description = "Some books which are very scary!"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Romance",
                    Description = "Yuk!"
                }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = Guid.Parse("cc49ae9a-c1a2-4226-a5a4-5627646afaf2"),
                    Title = "Scary story",
                    Author = "Old vampire",
                    PublicationYear = 2004,
                    Quantity = 3,
                    ISBN = "978-617-7171-80-4",

                    CategoryId = categoryGuid,
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Not scary story",
                    Author = "Witch next door",
                    PublicationYear = 2010,
                    Quantity = 0,
                    ISBN = "978-617-3121-80-4",

                    CategoryId = categoryGuid,
                }
            );
        }
    }
}
