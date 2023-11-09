using API.FurnitoreStore.API.Configuration;
using API.FurnitoreStore.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<APIFurnitureStoreContext>(options => 
                options.UseSqlite(builder.Configuration.GetConnectionString("APIFurnitureStoreContext")));

//Inyecta dependencia para el Jwt desde secrets
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

//Seteamos la autenticacion para jwt baerer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    //Traemos la key codificada
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);
    //Guardamos el token
    jwt.SaveToken = true;
    //instanciamos para validar
    jwt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        //la validacion tiene que suceder
        ValidateIssuerSigningKey = true,
        //la validacion que tiene que hacer
        IssuerSigningKey = new SymmetricSecurityKey(key),
        //solo en entorno de desarrollo false, luego true. LAS TRES
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true,
    };
    
});

//para usar IdentityUser como clase (para autenticacion)
//(se deja falso para testear, en produccion pasa a true)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                                                                   .AddEntityFrameworkStores<APIFurnitureStoreContext>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Add autorizacion del builder agregada
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
