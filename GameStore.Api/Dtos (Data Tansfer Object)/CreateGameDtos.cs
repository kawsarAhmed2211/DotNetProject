namespace GameStore.Api.Dtos;

public record CreateGameDtos
(
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);