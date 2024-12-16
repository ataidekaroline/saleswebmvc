using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;
using SalesWebMvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SalesWebMvcContext") ?? throw new InvalidOperationException("Connection string 'SalesWebMvcContext' not found.")));

// Registrar o SellerService
builder.Services.AddScoped<SellersService>();

// Registrar o SeedingService
builder.Services.AddScoped<SeedingService>();

// Registrar o Department
builder.Services.AddScoped<DepartmentService>();

// Adicionar controllers e views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Aplica as migrations e chama o seeding no desenvolvimento
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        // Aplica as migrations automaticamente
        var dbContext = scope.ServiceProvider.GetRequiredService<SalesWebMvcContext>();
        dbContext.Database.Migrate(); // Aplica as migrations automaticamente

        // Chama o método de Seed para adicionar dados
        var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
        seedingService.Seed(); // Chama o método Seed
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // The default HSTS value is 30 days. You may want to change this for production scenarios.
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configuração da rota do Controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
