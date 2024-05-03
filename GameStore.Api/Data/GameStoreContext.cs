using GameStore.Api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

//use identiyDbContext: generate the tables which will be stored the credentials, roles, and stuff, 
//Inherits from IdentityDbContext to integrate the authentication and authorization functionality of ASP.NET Core Identity
public class GameStoreContext : IdentityDbContext
{
    public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options)
    {
    }

    //this properties represent the tables in the dataBase. DbSet provide Crud Functionalities in the tables.
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

    //this method is going to be exucted when we make the migrations
    // The OnModelCreating method is used to configure the data model. 
    // The HasData method is used to specify data that should be inserted into the database 
    // when it is first created, such as initial records for reference tables.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //// Calls the base class's OnModelCreating method to configure the data model for ASP.NET Core Identity,
        // including setting up tables and relationships for user management.
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "Fighting" },
            new { Id = 2, Name = "Roleplaying" },
            new { Id = 3, Name = "Sports" },
            new { Id = 4, Name = "Racing" },
            new { Id = 5, Name = "Kids and Family" }
        );
    }
}
