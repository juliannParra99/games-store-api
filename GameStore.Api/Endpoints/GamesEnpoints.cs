using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEnpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = new()
    {
        new GameDto(
            Id: 1,
            Name: "Street Fighter II",
            Genre: "Fighting",
            Price: 19.99M,
            ReleaseDate: new DateOnly(1992, 7, 15)),
        new GameDto(
            Id: 2,
            Name: "Final Fantasy XIV",
            Genre: "Roleplaying",
            Price: 59.99M,
            ReleaseDate: new DateOnly(2010, 9, 30)),
        new GameDto(
            Id: 3,
            Name: "FIFA 23",
            Genre: "Sports",
            Price: 69.99M,
            ReleaseDate: new DateOnly(2022, 9, 27))

    };

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        //as we've been using "games" to define our routes, with MapGroup We can define the route just once and using it when it's neccesary + WithParameterValidation() yo apply the required stuff we applied into our dtos
        var group = app.MapGroup("games").WithParameterValidation();

        //GET /Games
        group.MapGet("/", () => games);

        //GET /games/1

        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);

        });

        //PUT /games
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
             id,
             updatedGame.Name,
             updatedGame.Genre,
             updatedGame.Price,
             updatedGame.ReleaseDate
            );
            return Results.NoContent();
        });

        //DELETE  /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;

    }

}
