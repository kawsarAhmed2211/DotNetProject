using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;


public static class GamesEndpoints
{
    const string GetGameEndPointName = "GetGame";
    private static readonly List<GameDtos> games = [
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
        group.MapGet("/", () => games);
        // second parameter in app.MapGet is a handler function that will be executed when the endpoint is called. In this case, it returns a simple string "Hello World!".

        //Get games by id 
        group.MapGet("/{id}", (int id) =>{
            var game = games.Find(game => game.Id == id);
            /*if (game == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(game);*/
            return game is  null ? Results.NotFound() : Results.Ok(game);
        }).WithName(GetGameEndPointName);


        //POST a game
        group.MapPost("/", (CreateGameDtos newGame, GameStoreContext dbContext) =>
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
            dbContext.SaveChanges();
            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );
            return Results.CreatedAtRoute(GetGameEndPointName, new {id = gameDto.Id}, gameDto);
        });

        // PUT /games/id
        group.MapPut("/{id}", (int id, UpdateGameDtos updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDtos(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE /games/id
        group.MapDelete("/{id}", (int id) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games.RemoveAt(index);

            return Results.NoContent();
        });

    }
}