
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// Configuração do DbContext para SQL Server
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SalesWebMvcContext") ?? throw new InvalidOperationException("Connection string 'SalesWebMvcContext' not found.")
    ));

// Registro dos serviços
builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<SellerService>(); // Nome atualizado para consistência
builder.Services.AddScoped<DepartmentService>();

// Adiciona suporte para controllers e views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração do pipeline de requisições
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Aplica as migrations automaticamente e faz o seeding
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SalesWebMvcContext>();
        dbContext.Database.Migrate(); // Aplica as migrations

        var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
        seedingService.Seed(); // Chama o método de seeding
    }
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Valor padrão de HSTS pode ser modificado para produção
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthorization();

// Configuração da rota padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
