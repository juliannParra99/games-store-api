using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Mapping;

public static class GameMapping
{
    // Create a new instance of Game with the data provided in newGame
    // Converts a CreateGameDto object to a Game object.

    public static Game ToEntity(this CreateGameDto game)
    {
        return new Game()
        {
            Name = game.Name,
            GenreId = game.GenreId, //use foreing key
            Price = game.Price,
            ReleaseDate = game.ReleaseDate

        };

    }

    // Create a new GameDto object with the data of the newly created game
    // Converts a Game object to a GameDto object.

    public static GameDto ToDto(this Game game)
    {
        return new(
            game.Id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.ReleaseDate
        );
    }
}
