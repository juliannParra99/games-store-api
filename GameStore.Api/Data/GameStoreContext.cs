using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    //this properties represent the tables in the dataBase. DbSet provide Crud Functionalities in the tables.
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

    //this method is going to be exucted when we make the migrations
    // The OnModelCreating method is used to configure the data model. 
    // The HasData method is used to specify data that should be inserted into the database 
    // when it is first created, such as initial records for reference tables.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "Fighting" },
            new { Id = 2, Name = "Roleplaying" },
            new { Id = 3, Name = "Sports" },
            new { Id = 4, Name = "Racing" },
            new { Id = 5, Name = "Kids and Family" }
        );
    }
}
