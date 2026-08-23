namespace GameStore.Api.Dtos;

public record UpdateGameDtos
(
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);