using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = new()
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

//GET /Games
app.MapGet("games", () => games);

//GET /games/1

app.MapGet("games/{id}", (int id) =>
{
    GameDto? game = games.Find(game => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);
}).WithName(GetGameEndpointName);

// POST /games
app.MapPost("games", (CreateGameDto newGame) =>
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
app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) =>
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
app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.Run();
