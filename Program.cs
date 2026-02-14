using System.Reflection;
using System.Text;
using bugdgetwarsapi.Authencation.Abstracts;
using bugdgetwarsapi.Authencation.Processors;
using bugdgetwarsapi.Database;
using bugdgetwarsapi.Database.Repositories;
using bugdgetwarsapi.DTOs;
using bugdgetwarsapi.Handlers;
using bugdgetwarsapi.Infrastructure.Services.account;
using bugdgetwarsapi.Models;
using bugdgetwarsapi.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(); 
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IAuthTokenProcessor, AuthTokenProcessor>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // React app URL
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//my ssqldb connection
 var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//jwtoptions
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.jwtOptionsKey));

//  Registers identity servies  handles roles  and password
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Optional: password rules
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders(); // enables password reset tokes and email confirmation tokens

// 3️⃣ Add authentication with cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login";      // redirect path if not logged in
    options.AccessDeniedPath = "/auth/access-denied";
});
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.GetSection(JwtOptions.jwtOptionsKey).Get<JwtOptions>() ??
                     throw new ArgumentException(nameof(JwtOptions));
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.secret))

    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["ACCESS_Token"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.WithTitle("budget wars  refresh token API");   
    });
    
}
app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthentication();  
app.UseAuthorization();
app.UseExceptionHandler(_ => { });

app.UseCors();







app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}