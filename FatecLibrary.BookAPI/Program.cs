using FatecLibrary.BookAPI.Context;
using FatecLibrary.BookAPI.Repositories.Entities;
using FatecLibrary.BookAPI.Repositories.Interfaces;
using FatecLibrary.BookAPI.Services.Entities;
using FatecLibrary.BookAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(
        c => c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// configurando Swagger para receber o Token
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FatecLibrary.BookAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter 'Bearer' [space] your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    }
    );

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string> ()
                }
            }
    );
}
);


// pegando a string de conexão
var mySqlConnection = builder
    .Configuration.GetConnectionString("DefaultConnection");

// usar para que o Entity Framework
// crie nossas tabelas no banco de dados
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseMySql(mySqlConnection, 
    ServerVersion.AutoDetect(mySqlConnection))
);

// garantir que todos os assemblies do domain sejam injetados
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// criando a injeção de dependencia
builder.Services.AddScoped<IPublishingRepository, PublishingRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IPublishingService, PublishingService>();
builder.Services.AddScoped<IBookService, BookService>();


// autenticacao
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["FatecLibrary.IdentityServer:ApplicationUrl"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, // para validar os servidores de recursos que devem acessar o Token
        };
    }
    );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "fateclibrary");
    });
}
    );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
