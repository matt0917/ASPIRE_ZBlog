using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ZBlogApp.ApiService.Services;
using ZBlogApp.BlazorServer.Data;
using ZBlogApp.Library;

var builder = WebApplication.CreateBuilder(args);

// Https configuration
// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ListenAnyIP(7591, listenOptions => listenOptions.UseHttps()); // activates https
//     options.ListenAnyIP(5350); // secondary http port for service discovery
// });

// Access various DB. types
if (builder.Configuration.GetValue("aspire", "true") == "true") {
    // Use Aspire provided connection name string
    builder.AddSqlServerDbContext<ApplicationDbContext>("ZBlogDB");
} else {
    var connectionString = builder.Configuration.GetConnectionString("DATABASE_CONNECTION_STRING") ?? throw new InvalidOperationException("Connection string 'DATABASE_CONNECTION_STRING' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// Add CRUD services
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
// Add default services configurations
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
// Add OpenAPI services
builder.Services.AddOpenApi();

// Add Cors
builder.Services.AddCors(o => o.AddPolicy("Policy", builder => {
  builder.AllowAnyOrigin() // For anyone to access
    .AllowAnyMethod() // Allow any method
    .AllowAnyHeader(); // Allow any headers
}));

var app = builder.Build(); //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// Use Cross Origin Resource Sharing
app.UseCors("Policy");

// Use OpenAPI in development environment
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => {
        options.Title = "ZBlogApp API";
        options.Theme = ScalarTheme.Kepler;
        options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
        options.ShowSidebar = true;
    });
}

// Enable HTTPS Redirection
app.UseHttpsRedirection();

// Auto-register endpoints of CRUD services
ArticleService.RegisterRoutes(app);
UserService.RegisterRoutes(app);
RoleService.RegisterRoutes(app);

// Register other default endpoints
app.MapDefaultEndpoints();

await app.RunAsync();
