namespace GameStore.Api.Dtos;

// A DTO (Data Transfer Object) is a simple object that is used to transfer data between different layers of an application. In this case, the GameDtos class is used to transfer data related to games in the GameStore application.
// in other words, it is a contract between the client and the server that defines the structure of the data that will be sent and received. 
// It is used to decouple the internal representation of the data from the external representation, allowing for more flexibility and maintainability in the application.
// a shared agreement about how data will be transferred between the client and the server. It defines the structure of the data, including the properties and their types, that will be sent and received. This allows both the client and the server to understand what data is expected and how it should be formatted, making it easier to communicate and work with the data.
public record GameDtos
(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
