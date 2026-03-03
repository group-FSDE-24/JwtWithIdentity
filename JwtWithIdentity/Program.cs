using JwtWithIdentity.Datas;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JwtWithIdentity.Models.Entities.Concretes;
using JwtWithIdentity.Services.Abstracts;
using JwtWithIdentity.Services.Concretes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JwtWithIdentity.BackgroundServices;
using JwtWithIdentity.Configurations;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.



// ---------------------------------------------

// Loglama 3 yolla ede bilerik:

// 1. Console
// 2. File
// 3. DB

// ---------------------------------------------

// --------------------------------------------- 

// Asp uzerinde gelen loglama


//builder.Services.AddLogging(c => c.AddConsole());

// ---------------------------------------------


// ---------------------------------------------

// Serilog ile loglama, Simple version

// LoggerConfiguration logger = new LoggerConfiguration();
// 
// logger.MinimumLevel.Debug();
// logger.WriteTo.Console();
// 
// Log.Logger = logger.CreateLogger();


// ---------------------------------------------

// ---------------------------------------------

var template = "[{Timestamp:HH:mm:ss} {Level:u5}] {Message:lj} {EnvironmentName} {ThreadId} {NewLine} {Exception}";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("Log/logger.txt")
    .WriteTo.Console(outputTemplate: template)
    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sinkOptions: new MSSqlServerSinkOptions()
    {
        TableName = "LogEvents",
        AutoCreateSqlTable = true
    })
    .Enrich.WithEnvironmentName()
    .Enrich.WithThreadId()
    .CreateLogger();


builder.Host.UseSerilog();

// ---------------------------------------------


// builder.Services.AddHostedService<MyBGService>();
// builder.Services.AddHostedService<SomeBGService>();

builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWT"));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT tokeni daxil edin. M?s?l?n: Bearer 12345abcdef"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// builder.Services.AddDbContext<AppDbContext>(option =>
// {
//     option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
// });

builder.Services.AddDbContextFactory<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/api/Auth/Login";
});

builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        LifetimeValidator = (notBefore, expires, token, param) => expires > DateTime.UtcNow,
                        ValidIssuer = builder.Configuration["JWT:IsSuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))

                    };
                });

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
