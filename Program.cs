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
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




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
    cfg.CreateMap<Permissions, PermissionDTO>();
    cfg.CreateMap<Turno, TurnoDTO>();
});

IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<RolePermissionsDTO>();
builder.Services.AddScoped<PermissionDTO>();
builder.Services.AddScoped<ITokenService,AuthenticationServiceJWT>();

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



//Authentication basada en tokens
builder.Services.AddScoped<AuthenticationServiceJWT>();

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

// ----------------------------------------

//IUserService
builder.Services.AddScoped<IUserService, UserService>();
//--------------

//Authorization basada en permisos






// Obtiene los permisos requeridos desde los atributos de autorización en el contexto HTTP


//builder.Services.AddScoped<PermissionAuthorizationFilter>(provider =>
//{
//    var permission = "AgregarUsuario"; // Reemplaza esto por el valor correcto
//    var dbContext = provider.GetRequiredService<MyDbContext>();
//    return new PermissionAuthorizationFilter(permission, dbContext);
//});

//builder.Services.AddControllers(options =>
//{
//    options.Filters.Add(new ServiceFilterAttribute(typeof(PermissionAuthorizationFilter)));
//});

//-------------------------------------
builder.Services.AddControllers();

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

//builder.Services.AddLogging(logging =>
//{
//    logging.ClearProviders();
//    logging.AddConsole();

//});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();



var app = builder.Build();


//MIDDLEWARE PARA RENOVAR TOKEN AUTOMATICAMENTE PERO SE IMPLEMENTO OTRA METODOLOGIA
//app.UseMiddleware<TokenValidationMiddleware>();

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

