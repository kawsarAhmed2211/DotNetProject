using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDtos> games = [
    new (1, "Street Fighter V", "Fighting", 19.99m, new DateOnly(2016, 2, 16)),
    new (2, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
    new (3, "Super Mario Odyssey", "Platformer", 49.99m, new DateOnly(2017, 10, 27)),
    new (4, "Red Dead Redemption 2", "Action-adventure", 59.99m, new DateOnly(2018, 10, 26)),
    new (5, "The Witcher 3: Wild Hunt", "Action role-playing", 39.99m, new DateOnly(2015, 5, 19)),
    new (6, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18))
];


// GET /games
app.MapGet("/games", () => "Hello World!");
// second parameter in app.MapGet is a handler function that will be executed when the endpoint is called. In this case, it returns a simple string "Hello World!".

// for creating any endpoints you need to create a file for example /games you need an endpoint with /games
app.Run();
