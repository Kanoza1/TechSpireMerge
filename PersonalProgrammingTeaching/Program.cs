using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalLearning.Helper;
using StackExchange.Redis;
using System.Text;

namespace PersonalLearning;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<IDentityUserDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration
                .GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddIdentity<AppUser, IdentityRole>(
            options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            }
        )
        .AddEntityFrameworkStores<IDentityUserDbContext>()
        .AddSignInManager<SignInManager<AppUser>>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:key"])),
                    ValidIssuer = builder.Configuration["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });

        builder.Services.AddControllers();

        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
            return ConnectionMultiplexer.Connect(configuration);
        });

        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IMailService, MailService>();
        builder.Services.AddScoped<IResponseCacheService, ResponseCacheService>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = Path.Combine(AppContext.BaseDirectory, "PersonalLearning.xml");
            c.IncludeXmlComments(xmlFile);
        });

        builder.Services.AddAutoMapper(typeof(ProfileMapping));
        builder.Services.AddCors(c => c.AddPolicy("CorsPolicy", c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true)));
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailSettings"));

        var app = builder.Build();
        app.UseStaticFiles(); // سيخدم الملفات من wwwroot
                              // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
