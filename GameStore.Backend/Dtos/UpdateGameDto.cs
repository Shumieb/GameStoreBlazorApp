using System.ComponentModel.DataAnnotations;

namespace GameStore.Backend.Dtos;

public record class UpdateGameDto(
    [Required][StringLength(50)] string Name,
    [Required] int GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);
