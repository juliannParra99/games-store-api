namespace GameStore.Api.Entities;

public class Game
{
    //properties for the columns of the data
    public int Id { get; set; }
    public required string Name { get; set; }

    //to asocciete the game with the data from Genre table
    public int GenreId { get; set; }

    public Genre? Genre { get; set; }

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

}
