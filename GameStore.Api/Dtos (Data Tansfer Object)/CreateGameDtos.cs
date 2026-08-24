using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record CreateGameDtos
(
     [Required(AllowEmptyStrings = false)]
     [StringLength(50)]
    string Name,

    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    string Genre,

    [Range(1,100) ]decimal Price,
    [Required ]DateOnly ReleaseDate
);