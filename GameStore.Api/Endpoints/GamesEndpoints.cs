using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;


public static class GamesEndpoints
{
    const string GetGameEndPointName = "GetGame";
    private static readonly List<SummaryGameDtos> games = [
        new (1, "Street Fighter V", "Fighting", 19.99m, new DateOnly(2016, 2, 16)),
        new (2, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
        new (3, "Super Mario Odyssey", "Platformer", 49.99m, new DateOnly(2017, 10, 27)),
        new (4, "Red Dead Redemption 2", "Action-adventure", 59.99m, new DateOnly(2018, 10, 26)),
        new (5, "The Witcher 3: Wild Hunt", "Action role-playing", 39.99m, new DateOnly(2015, 5, 19)),
        new (6, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18))
    ];

    public static void MapGamesEndpoints (this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        //app.MapGet("/games", () => "Hello World!");
        group.MapGet("/", async (GameStoreContext dbContext) => 
        await dbContext.Games.Include(game => game.Genre).Select(game => new SummaryGameDtos(
            game.Id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.ReleaseDate
        )).ToListAsync()
        );
        // second parameter in app.MapGet is a handler function that will be executed when the endpoint is called. In this case, it returns a simple string "Hello World!".

        //Get games by id 
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext ) =>{
            //var game = games.Find(game => game.Id == id);
            var game = await dbContext.Games.FindAsync(id);
            /*if (game == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(game);*/
            return game is  null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            );
        }).WithName(GetGameEndPointName);


        //POST a game
        group.MapPost("/", async (CreateGameDtos newGame, GameStoreContext dbContext) =>
        {
            // if(string.IsNullOrEmpty(newGame.Name) || string.IsNullOrEmpty(newGame.Genre) || newGame.Price <= 0)
            // //you can do the above code but it will take too much lines of code best way is done on createGameDto
            // {
            //     return Results.BadRequest("Invalid game data.");
            // }

            /*GameDtos game = new (
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
            */

            Game game = new (){
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            //games.Add(game);
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            GameDetailsDto gameDetailsDto = new (
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );
            return Results.CreatedAtRoute(GetGameEndPointName, new {id = gameDetailsDto.Id}, gameDetailsDto);
        });

        // PUT /games/id
        group.MapPut("/{id}", async (int id, UpdateGameDtos updatedGame, GameStoreContext dbContext) =>
        {

            var existingGame = await dbContext.Games.FindAsync(id);

            //var index = games.FindIndex(game => game.Id == id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            /*games[index] = new SummaryGameDtos(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );*/
            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /games/id
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Games.Remove(existingGame);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

    }
}