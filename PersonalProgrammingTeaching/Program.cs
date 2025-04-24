
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.X509.Qualified;
using PersonalLearning.Controllers;
using PersonalLearning.Helper;
using PersonalLearning.Specifications;
using StackExchange.Redis;
using System.Text;

namespace PersonalProgrammingTeaching
{
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
            }
                );

          
            builder.Services.AddIdentity<AppUser,IdentityRole>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = true;
                   options.User.AllowedUserNameCharacters= "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
                    
                }
                )
                .AddEntityFrameworkStores<IDentityUserDbContext>(
                
                )
                .AddSignInManager< SignInManager<AppUser>>( );
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer
                (
               options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey
                       (Encoding.UTF8.GetBytes(builder.Configuration["Token:key"])),
                       ValidIssuer = builder.Configuration["Token:Issuer"],
                       ValidateIssuer = true,
                       ValidateAudience = false
                   };
               }
               );
            builder.Services.AddControllers();
           // builder.Logging.ClearProviders();
           // builder.Logging.AddConsole();

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions
                .Parse(builder.Configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(configuration);
            });
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            builder.Services.AddScoped<IResponseCacheService, ResponseCacheService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped(typeof( IGenericRepository<>),typeof(GenericRepository<>));
            // builder.Services.AddScoped(typeof(ISpecification),typeof(BaseSpecification<>));

            builder.Services.AddScoped(typeof(IdentityController));

            //builder.Services.AddScoped<ILogger<IdentityController>>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(ProfileMapping));
            builder.Services.AddCors(c => c.AddPolicy("CorsPolicy", c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true)));
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailSettings"));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
           /* if (app.Environment.IsDevelopment())
            {*/
                app.UseSwagger();
                app.UseSwaggerUI();
                
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
