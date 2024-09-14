using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<VendingMachineContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirstConnection")));
Console.WriteLine(builder.Configuration.GetConnectionString("FirstConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Store}/{action=Index}/{id?}");

app.Run();
