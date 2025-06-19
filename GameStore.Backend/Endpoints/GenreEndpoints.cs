using GameStore.Backend.Data;
using GameStore.Backend.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Backend.Endpoints;

public static class GenreEndpoints
{
    public static RouteGroupBuilder MapGenreEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("genres");

        // GET /genres
        group.MapGet("/", async (GameStoreContext dbcontext) =>
                await dbcontext.Genres
                                .Select(genre => genre.ToDto())
                                .AsNoTracking()
                                .ToListAsync());

        return group;
    }
}
