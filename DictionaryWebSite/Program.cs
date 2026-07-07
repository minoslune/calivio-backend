using CloudinaryDotNet;
using DictionaryWebSite.Data;
using DictionaryWebSite.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//Cloudinary configuration start ///////////////////////////////////////////////////////////////////////////////////////////////////
var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary");
var account = new Account(
    cloudinaryConfig["CloudName"], //The quoted values in the appsettings.json file are accessed using the indexer syntax with the corresponding keys.
    cloudinaryConfig["ApiKey"],
    cloudinaryConfig["ApiSecret"]
);
var cloudinary = new Cloudinary(account);
cloudinary.Api.Secure = true;
//adding cloudinary to the dependency injection container as a singleton, meaning that the same instance will be used throughout the application's lifetime. This is appropriate for services like Cloudinary that manage resources and maintain state, ensuring efficient use of resources and consistent behavior across the application.
builder.Services.AddSingleton(cloudinary);
//Cloudinary configuration end //////////////////////////////////////////////////////////////////////////////////////////////////

//Db code start ///////////////////////////////////////////////////////////////////////////////////////////////////
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//adding CORS policy
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("MyCors", builder =>
    {       
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

// Add authentication IDENTITY
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();
//End of authentication IDENTITY



//THIS ADDS THE EMPLOYEE REPOSITORY TO THE DEPENDENCY INJECTION
//This creates a new employeeRepository instance for the lifetime of a single HTTP request, after
//the request is done, the instance is removed.
//Just ask for the dependency on the constructor like Constructor(IEmployeeRepository EmployeeRepository)
builder.Services.AddScoped<IWordRepository, WordRepository>();

//DB code End //////////////////////////////////////////////////////////////////////////////////////////////////

// Add services to the container.

builder.Services.AddControllers();

//adding swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//



var app = builder.Build();

//PART OF AUTHENTIFICATION SETUP
// ... after builder.Build(): 
app.UseAuthentication(); // must come before UseAuthorization
app.UseAuthorization();
//End of authentification setup

//only use swagger on development enviroment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        //This declares the direction of our API in the URL
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        //This makes the default URL go to just /swagger for testing.
        c.RoutePrefix = string.Empty;
    }
    );
}


//app.UseHttpsRedirection();

//app.UseAuthorization();

app.UseCors("MyCors");

app.MapControllers();

app.Run();
