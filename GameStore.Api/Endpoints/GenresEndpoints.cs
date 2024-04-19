using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    //Configures the endpoints related to game genres for the application.The WebApplication instance to configure the endpoints for; A RouteGroupBuilder for further endpoint configuration
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        //group for all endpoints related to game genres
        var group = app.MapGroup("genres");

        // Configure a GET endpoint for the base route ("/") to retrieve all genres
        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                .Select(genre => genre.ToDto())
                .AsNoTracking()
                .ToListAsync());

        return group;
    }

}
