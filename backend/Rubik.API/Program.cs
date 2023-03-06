using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Rubik.API;
using Rubik.API.Models;
using Rubik.API.Services;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins, policy => { policy.WithOrigins("http://localhost:3000"); });
});

builder.Services.AddAuthentication(auth => auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ClockSkew = TimeSpan.Zero,
    });

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IScoresService, ScoresService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IRankingsService, RankingsService>();
builder.Services.AddScoped<ITutorialsService, TutorialsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureDeleted();
    Console.WriteLine(context.Database.EnsureCreated());

    var users = new[]
    {
        new UserEntity(1, "Andrzej", BCrypt.Net.BCrypt.HashPassword("Andrzej-1"), "andrzej@gmail.com"),
        new UserEntity(2, "Ania", BCrypt.Net.BCrypt.HashPassword("Andrzej-1"), "ania@gmail.com"),
        new UserEntity(3, "Piotr", BCrypt.Net.BCrypt.HashPassword("Andrzej-1"), "piotr@gmail.com")
    };

    users[0].Scores = new List<ScoreEntity>
    {
        new ScoreEntity(1, 352, new DateTime(2022, 8, 21, 16, 28, 32), 1),
        new ScoreEntity(2, 322, new DateTime(2022, 8, 23, 18, 28, 32), 1),
        new ScoreEntity(3, 327, new DateTime(2022, 8, 23, 12, 28, 32), 1),
        new ScoreEntity(4, 237, new DateTime(2022, 8, 24, 16, 28, 32), 1),
    };

    users[1].Scores = new List<ScoreEntity>
    {
        new ScoreEntity(5, 480, new DateTime(2022,11, 21, 16, 28, 32), 2),
    };

    var ranking = new[]
    {
        new RankingEntity(1, 1, 4),
        new RankingEntity(2, 2, 5)
    };

    var tutorialPages = new[]
    { 
        new TutorialPageEntity(1, "Notacja"),
        new TutorialPageEntity(2, "Biały krzyż"),
        new TutorialPageEntity(3, "Białe narożniki"),
        new TutorialPageEntity(4, "Środkowa warstwa"),
        new TutorialPageEntity(5, "Górny krzyż"),
        new TutorialPageEntity(6, "Permutacja górnych krawędzi"),
        new TutorialPageEntity(7, "Permutacja górnych narożników"),
        new TutorialPageEntity(8, "Orientacja górnych narożników")
    };

    tutorialPages[0].Sections = new List<TutorialSectionEntity>
    {
        new TutorialSectionEntity(1, "Oznaczenia liter", "F - Przód (Front)\nR - Prawa (Right)\nU - Góra (Up)\nL - Lewa (Left)\nD - Dół (Down)", 1),
        new TutorialSectionEntity(2, "", "Litera oznacza obrót danej sciany zgodnie z ruchem wskazówek zegara, natomiast litera z apostrofem oznacza obrót danej sciany przeciwnie do ruchu wskazówek zegara", 1),
    };

    tutorialPages[1].Sections = new List<TutorialSectionEntity>
    { 
        new TutorialSectionEntity(3, 
        "Zaczynamy od zbudowania białego krzyża", "U' R' U F' - Wykonaj ten algorytm, gdy krawędź jest w dobrym miejscu (krawędź FU), ale jest źle obrócona.\nF' U' R U - Wykonaj ten algorytm, jeśli po przekręceniu frontowej ściany krawędź znalazłaby się na swoim miejscu, ale byłaby źle obrócona.", 2)
    };

    context.Users.AddRange(users);
    context.Scores.AddRange(users[0].Scores);
    context.Scores.AddRange(users[1].Scores);
    context.Rankings.AddRange(ranking);
    context.TutorialPages.AddRange(tutorialPages);
    context.TutorialSections.AddRange(tutorialPages[0].Sections);
    context.TutorialSections.AddRange(tutorialPages[1].Sections);

    context.SaveChanges();
}

//app.UseCors(myAllowSpecificOrigins);
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }