using Last.Simple.App.Domain.Contracts.Queries;
using Last.Simple.App.Domain.Contracts.Repositories;
using Last.Simple.App.Domain.Contracts.Services;
using Last.Simple.App.Domain.Contracts.UseCases.Products;
using Last.Simple.App.Domain.Contracts.UseCases.Users;
using Last.Simple.App.Infra;
using Last.Simple.App.Infra.Queries;
using Last.Simple.App.Infra.Repositories;
using Last.Simple.App.Services;
using Last.Simple.App.UseCases.Products;
using Last.Simple.App.UseCases.Users;
using Last.Simple.App.Utils.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
        .AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.SaveToken = true;

            if (builder.Environment.IsDevelopment())
            {
                opt.RequireHttpsMetadata = false;
            }

            var configuration = builder.Configuration;
            opt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? ""))
            };
        });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<DatabaseBuilder>();
builder.Services.AddScoped<ISQLConnectionBuilder, SQLConnectionBuilder>();
builder.Services.AddScoped<ILoggedUserService, LoggedUserService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICreateUserUC, CreateUserUC>();
builder.Services.AddScoped<ISignInUC, SignInUC>();

builder.Services.AddScoped<ICreateProductUC, CreateProductUC>();
builder.Services.AddScoped<IUpdateProductUC, UpdateProductUC>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductQuery, ProductQuery>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(config => 
    {
        config.AllowAnyOrigin();
        config.AllowAnyMethod();
        config.AllowAnyHeader();
    });

    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var dbBuilder = services.GetRequiredService<DatabaseBuilder>();
        await dbBuilder.Build();
    }
}

app.UseHandlingExtension();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
