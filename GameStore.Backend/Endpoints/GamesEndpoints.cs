using GameStore.Backend.Data;
using GameStore.Backend.Dtos;
using GameStore.Backend.Entities;
using GameStore.Backend.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Backend.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                        .WithParameterValidation();

        // GET /games
        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Games
                        .Include(game => game.Genre)
                        .Select(games => games.ToGameSummaryDto())
                        .AsNoTracking()
                        .ToListAsync());

        // GET /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();      
        
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
           
            return Results.CreatedAtRoute(
                GetGameEndpointName,
                new { id = game.Id },
                game.ToGameDetailsDto()
            );
        });

        // PUT /games/1
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContent) =>
        {
            var existingGame = await dbContent.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            };

            dbContent.Entry(existingGame)
                        .CurrentValues
                        .SetValues(updatedGame.ToEntity(id));

            await dbContent.SaveChangesAsync();

            return Results.NoContent();
        });

        //DELETE /games/1
        group.MapDelete("/{id}",async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                        .Where(game => game.Id == id)
                        .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
}
