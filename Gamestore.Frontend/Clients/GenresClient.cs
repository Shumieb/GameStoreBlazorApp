using Gamestore.Frontend.Models;

namespace Gamestore.Frontend.Clients;

public class GenresClient
{
    private readonly Genre[] genres =
    [
        new(){
            Id =1,
            Name="Action"
        },
        new(){
            Id =2,
            Name="Adventure"
        },
        new(){
            Id =3,
            Name="RPG"
        }
    ];

    public Genre[] GetGenres() => genres;
}
