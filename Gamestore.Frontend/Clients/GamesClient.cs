using Gamestore.Frontend.Models;

namespace Gamestore.Frontend.Clients;

public class GamesClient
{
    private readonly List<GameSummary> games =
    [
        new(){
                Id = 1,
                Name = "Game One",
                Genre = "Action",
                Price = 29.99m,
                ReleaseDate = new DateOnly(2021, 1, 1)
            },
        new(){
                Id = 2,
                Name = "Game Two",
                Genre = "Adventure",
                Price = 39.99m,
                ReleaseDate = new DateOnly(2022, 2, 2)
            },
        new(){
                Id = 3,
                Name = "Game Three",
                Genre = "RPG",
                Price = 49.99m,
                ReleaseDate = new DateOnly(2023, 3, 3)
            },
    ];

    private readonly Genre[] genres = new GenresClient().GetGenres();

    public GameSummary[] GetGames() => [.. games];

    public void AddGame(GameDetails game)
    {

        ArgumentException.ThrowIfNullOrWhiteSpace(game.GenreId);
        var genre = genres.Single(genre => genre.Id == int.Parse(game.GenreId));

        var gameSummary = new GameSummary
        {
            Id = games.Count + 1,
            Name = game.Name,
            Genre = genre.Name,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };

        games.Add(gameSummary);
    }
}
