using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<VendingMachineContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirstConnection")));

// Включаем сессии
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Добавляем сессии в конвейер
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Store}/{action=Index}/{id?}");

app.Run();
