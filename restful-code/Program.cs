using GalleryManagement.Core.Interfaces;
using GalleryManagement.Core.Mapping;
using GalleryManagement.Data.DataContext;
using GalleryManagement.Data.Repositories;
using GalleryManagement.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using restful_code.Mapping;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddDbContext<GalleryDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? @"Server=(localdb)\MSSQLLocalDB;Database=GalleryManagementDB"));

// Repositories
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IArtworkRepository, ArtworkRepository>();
builder.Services.AddScoped<IExhibitionRepository, ExhibitionRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

// Services
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IArtworkService, ArtworkService>();
builder.Services.AddScoped<IExhibitionService, ExhibitionService>();
builder.Services.AddScoped<ISaleService, SaleService>();

// AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddMaps(new[]
    {
        typeof(MappingProfile).Assembly,
        typeof(ApiMappingProfile).Assembly
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();