using Microsoft.EntityFrameworkCore;
using TravelRequestApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddNpgsqlDbContext<AppDbContext>("travelrequests");

builder.Services.AddScoped<ITravelRequestService, TravelRequestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetService<AppDbContext>()!;
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.MapTravelRequestEndPoints();
app.MapDefaultEndpoints();

app.Run();
