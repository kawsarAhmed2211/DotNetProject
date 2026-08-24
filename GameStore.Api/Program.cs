
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();
var app = builder.Build();

app.MapGamesEndpoints();

// for creating any endpoints you need to create a file for example /games you need an endpoint with /games
app.Run();
