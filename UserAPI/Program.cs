using BusinessLayer.Service;
using BusinessLayer.ServiceImpl;
using DataLayer.Constants.dbConnection;
using DataLayer.Constants.JwtTokenGen;
using DataLayer.Constants.Mapping;
using DataLayer.Interface;
using DataLayer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
namespace UserAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var jwtSettings = builder.Configuration.GetSection("JWT");
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = jwtSettings["Issuer"], // Set your issuer
            //            ValidAudience = jwtSettings["Audience"], // Set your audience
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])) // Set your secret key
            //        };
            //    });
            builder.Services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme
                ).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//valideates the server
                        ValidateAudience = true,//validates the user
                        ValidateLifetime = true,//validatwe the key is vali or within the expiering time
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        ValidAudience = builder.Configuration["JWT:ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:secret"]))
                    };
                })
                ;
            // Add services to the container.
            builder.Services.AddDbContext<DataContext>(
                    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            builder.Services.AddSwaggerGen(mid =>
            {
                mid.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "JWTToken_Auth_API"
                });
                mid.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter The JWT Token with bearer formet like bearer[space] token"
                });
                mid.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference=new OpenApiReference()
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(typeof(UserProfile), typeof(AddressProfile));
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IUser, UserRepository>();
            builder.Services.AddScoped<IUserService,UserServiceImpl>();

            
            builder.Services.AddScoped<IAddress, AddressRepository>();
            builder.Services.AddScoped<LoginRepository>();
            builder.Services.AddScoped<TokenGenerater>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Userid", policy =>
                    policy.RequireClaim(ClaimTypes.Sid));//I am using this claim from jwt by using police
            });
            var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();//<--  This enables JWT authentication
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
