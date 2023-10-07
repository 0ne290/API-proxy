using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using static PublicApi.Models.DatabaseContext;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PublicApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<MyDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("MySql"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))), ServiceLifetime.Scoped, ServiceLifetime.Scoped);
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.KEY,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromSeconds(1),
                    NameClaimType = "MerchantGuid",
                    RoleClaimType = "IdentityRole"
                };
            });

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}