using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using Microsoft.AspNetCore.Identity.UI.Services; // For IEmailSender
using MyApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Configure the database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<CartDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<LoginDBContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<OrderDBContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Email settings and services
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailSender, EmailService>(); // ✅ Proper injection for IEmailSender

// Configure Forwarded Headers (for proxies / load balancers)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.AddSignalR();
var app = builder.Build();
app.UseForwardedHeaders();
app.MapHub<OrderHub>("/orderHub");

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Define the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Product}/{id?}");

// Apply pending migrations at startup (optional)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
