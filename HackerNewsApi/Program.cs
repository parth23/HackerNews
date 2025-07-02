using HackerNewsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "HackerNews API", Version = "v1" });
});

// Configure HttpClient for HackerNews API
builder.Services.AddHttpClient<IHackerNewsService, HackerNewsService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Add Memory Caching
builder.Services.AddMemoryCache();

// Add CORS policy for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Register services with dependency injection
builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HackerNews API V1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hacker News API is running. Navigate to /swagger for documentation.");

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
