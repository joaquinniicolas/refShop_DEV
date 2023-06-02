using Microsoft.EntityFrameworkCore;
using refShop_DEV.Models.MyDbContext;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using refShop_DEV;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using refShop_DEV.Profiles;
using refShop_DEV.Models.Login;
using refShop_DEV.Services.Interfaces;
using refShop_DEV.Services;
using refShop_DEV.Models.Restaurant;
using Microsoft.AspNetCore.Http.Features;
using refShop_DEV.Models.Permission;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



//DB Service
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Service AutoMapper
builder.Services.AddAutoMapper(typeof(Startup));
MapperConfiguration config = new MapperConfiguration(cfg =>
{

    cfg.CreateMap<User, UserDto>();
    cfg.CreateMap<Mesa, MesaDTO>();
    cfg.CreateMap<UserRole, UserRoleDto>();
    cfg.CreateMap<RolePermissions, RolePermissionsDTO>();
    cfg.CreateMap<ActivityPermission, PermissionDTO>();
});

IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

//For Files
builder.Services.Configure<IISServerOptions>(
    options =>
    {
        options.AllowSynchronousIO = true; // Habilita el uso de métodos sincrónicos para lectura de archivos grandes
    });

builder.Services.Configure<FormOptions>(
    options =>
    {
        options.MultipartBodyLengthLimit = 104857600;
    });




builder.Services.AddScoped<AuthenticationServiceJWT>();

//Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

//IUserService
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .WithExposedHeaders("Location");
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseResponseCaching();


app.Run();

