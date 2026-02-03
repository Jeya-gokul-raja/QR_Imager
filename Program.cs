using Backend;
using Backend.Service;
using Microsoft.Extensions.FileProviders;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowReactApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") // React app URL
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddSingleton<ImageService>();

builder.Services.AddSingleton<IMongoClient>(
    new MongoClient("mongodb://localhost:27017"));





builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

IMongoClient client = new MongoClient("mongodb://localhost:27017");

builder.Services.AddSingleton<IMongoClient>(
    new MongoClient(builder.Configuration["MongoDB:ConnectionString"]));
builder.Services.AddSingleton<ImageService>();

var app = builder.Build();
app.UseStaticFiles();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors("AllowReactApp");
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "images")),
    RequestPath = "/images"
});



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

