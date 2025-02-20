using FinancialAssistent.Entities;
using FinancialAssistent.Helpers;
using FinancialAssistent.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Database>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<Database>();

builder.Services.AddHttpClient<MonobankService>(client =>
{
    client.BaseAddress = new Uri("https://api.monobank.ua/");
});

builder.Services.AddControllers();

builder.Services.AddScoped<BankCardService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<WidgetService>();
builder.Services.AddScoped<UserInfoService>();
builder.Services.AddScoped<BalanceInfoService>();
builder.Services.AddScoped<MonobankUpdater>();
builder.Services.AddScoped<MonobankBackgroundUpdater>();
builder.Services.AddScoped<MonthlyBudgetService>();
builder.Services.AddScoped<CostLimitsService>();
builder.Services.AddScoped<MonobankHttpClient>();
builder.Services.AddScoped<MonobankService>();

builder.Services.AddScoped<DashboardModelGenerator>();
builder.Services.AddScoped<ProfileInfoModelGenerator>();

builder.Services.AddScoped<MonobankUpdater>();
builder.Services.AddHostedService<MonobankBackgroundUpdater>();


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

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
