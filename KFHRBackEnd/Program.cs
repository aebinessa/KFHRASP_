using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var secretKey = "YOUR_SECRET_KEY"; // Ensure you have a strong secret key
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

builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
{
    Version = "v1",
    Title = "KFH-API",
    Description = "Your Api Description.",
    TermsOfService = new Uri("https://google.com"),
    Contact = new Microsoft.OpenApi.Models.OpenApiContact
    { Name = "Contact Title", Email = "contactemailaddress@domain.com", Url = new Uri("https://google.com") }
});
var filePath = Path.Combine(AppContext.BaseDirectory, "KFHController.cs");


c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    Description = @"JWT Authorization header using the Bearer scheme. Example: 'Bearer 12345abcdef'",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
});






    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                            Reference = new OpenApiReference
                                {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
});






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Use custom error handling middleware in production.
    app.UseHsts(); // Enforce HTTPS in production.
}

// These middleware are now correctly placed after the app build.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensures the authentication middleware is used
app.UseAuthorization(); // Ensures the authorization middleware is used

app.MapControllers();

app.Run();
