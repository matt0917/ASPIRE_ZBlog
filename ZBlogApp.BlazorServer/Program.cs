using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeedIdentity.Data;
using ZBlogApp.BlazorServer.Components;
using ZBlogApp.BlazorServer.Components.Account;
using ZBlogApp.BlazorServer.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// Access various DB. types
if (builder.Configuration.GetValue<bool>("aspire")) {
    // Use Aspire provided connection name string
    Console.WriteLine("*************************************Use ASPIRE SqlServer");
    builder.AddSqlServerDbContext<ApplicationDbContext>("ZBlogDB");
} else {
    Console.WriteLine("*************************************Use LOCAL_TEST SqlServer");
    var connectionString = builder.Configuration.GetConnectionString("DATABASE_CONNECTION_STRING") ?? throw new InvalidOperationException("Connection string 'DATABASE_CONNECTION_STRING' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();    
    await context.Database.MigrateAsync();

    var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();  
    var roleMgr = services.GetRequiredService<RoleManager<ApplicationRole>>();  

    SeedData.Initialize(context, userMgr, roleMgr).Wait();
}

await app.RunAsync();
