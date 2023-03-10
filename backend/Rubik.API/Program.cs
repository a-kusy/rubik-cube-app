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
        new TutorialPageEntity(2, "Bia??y krzy??"),
        new TutorialPageEntity(3, "Bia??e naro??niki"),
        new TutorialPageEntity(4, "??rodkowa warstwa"),
        new TutorialPageEntity(5, "G??rny krzy??"),
        new TutorialPageEntity(6, "Permutacja g??rnych kraw??dzi"),
        new TutorialPageEntity(7, "Permutacja g??rnych naro??nik??w"),
        new TutorialPageEntity(8, "Orientacja g??rnych naro??nik??w")
    };

    tutorialPages[0].Sections = new List<TutorialSectionEntity>
    {
        new TutorialSectionEntity(1, "Oznaczenia liter", "F - Prz??d (Front)\nR - Prawa (Right)\nU - G??ra (Up)\nL - Lewa (Left)\nD - D???? (Down)", 1),
        new TutorialSectionEntity(2, "", "Litera oznacza obr??t danej sciany zgodnie z ruchem wskaz??wek zegara, natomiast litera z apostrofem oznacza obr??t danej sciany przeciwnie do ruchu wskaz??wek zegara", 1),
    };

    tutorialPages[1].Sections = new List<TutorialSectionEntity>
    { 
        new TutorialSectionEntity(3, 
        "Zaczynamy od zbudowania bia??ego krzy??a", "U' R' U F' - Wykonaj ten algorytm, gdy kraw??d?? jest w dobrym miejscu (kraw??d?? FU), ale jest ??le obr??cona.\nF' U' R U - Wykonaj ten algorytm, je??li po przekr??ceniu frontowej ??ciany kraw??d?? znalaz??aby si?? na swoim miejscu, ale by??aby ??le obr??cona.", 2)
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