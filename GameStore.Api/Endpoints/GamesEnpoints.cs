using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEnpoints
{
    const string GetGameEndpointName = "GetGame";


    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        //as we've been using "games" to define our routes, with MapGroup We can define the route just once and using it when it's neccesary + WithParameterValidation() yo apply the required stuff we applied into our dtos
        var group = app.MapGroup("games").WithParameterValidation();

        //GET /Games
        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Games.Include(game => game.Genre).Select(game => game.ToGameSummaryDto()).AsNoTracking().ToListAsync())
            .RequireAuthorization();

        //GET /games/1

        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        }).WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();

            //add new game into the Db
            dbContext.Games.Add(game);
            //save changes into the database
            await dbContext.SaveChangesAsync();


            // Return an HTTP 201 Created result along with the URL to access the newly created game
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());

        });

        //PUT /games
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            // updates a Game entity in your database based on the values provided in an UpdateGameDto object.
            dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //DELETE  /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            //batch delete
            await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });

        return group;

    }

}
