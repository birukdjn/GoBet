using GoBet.Application.Hubs;
using GoBet.Application.Interfaces.Repositories;
using GoBet.Application.Interfaces.Services;
using GoBet.Application.Services;
using GoBet.Domain.Entities;
using GoBet.Infrastructure;
using GoBet.Infrastructure.Configuration;
using GoBet.Infrastructure.Identity;
using GoBet.Infrastructure.Notifications;
using GoBet.Infrastructure.Repositories;
using GoBet.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// 1. Controllers & Swagger
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    // JWT Authorization in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

// 2. CORS
services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 3. DB Context
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. Identity
services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

services.Configure<PasswordHasherOptions>(options =>
{
    options.IterationCount = 600000;
});

services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromHours(1));

// Email settings
services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


services.AddSignalR();

// 5. Application services
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<ITokenService, TokenService>();
services.AddScoped<IDriverService, DriverService>();
services.AddScoped<IEmailService, EmailService>();
services.AddScoped<ITripRepository, TripRepository>();
services.AddScoped<IBookingRepository, BookingRepository>();
services.AddScoped<ITerminalRepository, TerminalRepository>();
services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ITripService, TripService>();
services.AddScoped<INotificationService, SignalRNotificationService>();

// 6. Authentication
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
    };
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.SignInScheme = IdentityConstants.ExternalScheme;
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
    options.SignInScheme = IdentityConstants.ExternalScheme;
});

var app = builder.Build();

// 7. Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 8. DB Migration & Seeding
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    await DbInitializer.SeedAsync(scope.ServiceProvider);
}

app.MapHub<TripHub>("/hubs/trip");

app.Run();

