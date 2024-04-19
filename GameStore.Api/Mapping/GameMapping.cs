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

    // Converts an instance of UpdateGameDto to a Game entity with the specified Id.
    //The UpdateGameDto instance to convert. The Id to assign to the resulting Game entity
    // A new Game entity initialized with the values from the UpdateGameDto and the specified Id.
    public static Game ToEntity(this UpdateGameDto game, int id)
    {
        return new Game()
        {
            Id = id,
            Name = game.Name,
            GenreId = game.GenreId, //use foreing key
            Price = game.Price,
            ReleaseDate = game.ReleaseDate

        };

    }

    // Create a new GameDto object with the data of the newly created game
    // Converts a Game object to a GameDto object.

    public static GameSummaryDto ToGameSummaryDto(this Game game)
    {
        return new(
            game.Id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.ReleaseDate
        );
    }

    public static GameDetailsDto ToGameDetailsDto(this Game game)
    {
        return new(
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate
        );
    }
}
