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
        group.MapGet("/", (GameStoreContext dbContext) =>
            dbContext.Games
                        .Include(game => game.Genre)
                        .Select(games => games.ToGameSummaryDto())
                        .AsNoTracking());

        // GET /games/1
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            Game? game = dbContext.Games.Find(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();      
        
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
           
            return Results.CreatedAtRoute(
                GetGameEndpointName,
                new { id = game.Id },
                game.ToGameDetailsDto()
            );
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContent) =>
        {
            var existingGame = dbContent.Games.Find(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            };

            dbContent.Entry(existingGame)
                        .CurrentValues
                        .SetValues(updatedGame.ToEntity(id));
            dbContent.SaveChanges();

            return Results.NoContent();
        });

        //DELETE /games/1
        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {
            dbContext.Games
                        .Where(game => game.Id == id)
                        .ExecuteDelete();

            return Results.NoContent();
        });

        return group;
    }
}
