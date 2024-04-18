using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Grupp2MVC.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Grupp2MVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Grupp2MVCContext") ?? throw new InvalidOperationException("Connection string 'Grupp2MVCContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Vehicles}/{action=Index}/{id?}");

app.Run();