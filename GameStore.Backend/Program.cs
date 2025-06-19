using GameStore.Backend.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games =
[
    new(1, "Game one", "Fighting", 19.99M, new DateOnly(2025, 5, 20)),
    new(2, "Game two", "RPG", 49.99M, new DateOnly(2025, 8, 10)),
    new(3, "Game three", "Adventure", 59.99M, new DateOnly(2024, 2, 12)),
    new(4, "Game four", "Fighting", 9.99M, new DateOnly(2023, 6, 01))
];

// GET /games
app.MapGet("/games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(GetGameEndpointName);

// POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

// PUT /games/1
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

//DELETE /games/1
app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.Run();
