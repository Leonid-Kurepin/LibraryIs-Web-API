using System;
using LibraryIS.Auth;
using LibraryIS.DAL.Entities;
using LibraryIS.DAL.Entities.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryIS.DAL
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberBook> MemberBooks { get; set; }

        [Obsolete] public static readonly ILoggerFactory ConsoleLoggerFactory =
            new LoggerFactory().AddConsole(LogLevel.Information);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(ConsoleLoggerFactory) //tie-up DbContext with LoggerFactory object
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new PublisherConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new MemberBookConfiguration());

            modelBuilder.Entity<User>().HasData
            (
                new User {Id = 1, Name = "admin", Email = "admin", Password = "111", Role = Role.Admin},
                new User
                {
                    Id = 2, Name = "Елена Сергеевна Менделеева", Email = "e.mendeleeva@library.ru", Password = "123",
                    Role = Role.User
                },
                new User
                {
                    Id = 3, Name = "Дмитрий Иванович Книголюбов", Email = "d.knigolyubov@library.ru", Password = "123",
                    Role = Role.User
                }
            );
        }
    }
}
