
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura��o de servi�os
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// Configura��o do DbContext para SQL Server
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SalesWebMvcContext") ?? throw new InvalidOperationException("Connection string 'SalesWebMvcContext' not found.")
    ));

// Registro dos servi�os
builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<SellerService>(); // Nome atualizado para consist�ncia
builder.Services.AddScoped<DepartmentService>();

// Adiciona suporte para controllers e views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configura��o do pipeline de requisi��es
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Aplica as migrations automaticamente e faz o seeding
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SalesWebMvcContext>();
        dbContext.Database.Migrate(); // Aplica as migrations

        var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
        seedingService.Seed(); // Chama o m�todo de seeding
    }
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Valor padr�o de HSTS pode ser modificado para produ��o
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthorization();

// Configura��o da rota padr�o
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
