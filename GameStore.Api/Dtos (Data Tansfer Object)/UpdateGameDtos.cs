using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record UpdateGameDtos
(
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    string Name,

    [Required(AllowEmptyStrings = false)]
    [Range(1,50)]
    int GenreId,

    [Range(1,100) ]decimal Price,
    [Required ]DateOnly ReleaseDate
);