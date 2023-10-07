using ApiProxy.Dal;
using ApiProxy.Logic;
using ApiProxy.Logic.Boundaries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace ApiProxy.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DependencyResolution(ServiceLocator.Current);
			AspConfigurationAndRun(args);
        }
		private static void DependencyResolution(IServiceLocator serviceLocator)
		{
			serviceLocator.Add<IRepository>(() => new DevelopmentRepository())
				.Add<HttpClient>(() => new HttpClient())
				.AddSingleton<ApiProxy.Logic.Tools>(() => new ApiProxy.Logic.Tools(serviceLocator))
				.AddSingleton<Accounting>(() => new Accounting(serviceLocator))
				.AddSingleton<Api>(() => new Api(serviceLocator, "https://api.staging.pay2play.cash", "https://MyDomain/Callback/1f8976d4-149e-4aa0-89aa-e766d89cfc7d/", "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIxMjQxMWYyNS1kN2IzLTQ1ZTQtOGJhNC0yNmUyZGFkMzU3N2MiLCJpYXQiOjE2OTA5NzUxMTQsImlzcyI6ImNvcmUtaWQiLCJzdWIiOiJ1c2VyIiwidWlkIjoxNTcwNywidmVyIjoxLCJyZXMiOlsxXSwidHlwIjoxLCJzY29wZXMiOlsiYXV0aDphY3Rpb24iLCJjb3JlOnJlYWQiLCJleGNoYW5nZTphY3Rpb24iLCJleGNoYW5nZTpyZWFkIiwibWVyY2hhbnQ6YWN0aW9uIiwibWVyY2hhbnQ6cmVhZCIsIm5vdGlmaWNhdGlvbnM6YWN0aW9uIiwibm90aWZpY2F0aW9uczpyZWFkIiwicGF5b3V0czphY3Rpb24iLCJwYXlvdXRzOnJlYWQiLCJwcm9maWxlOmFjdGlvbiIsInByb2ZpbGU6cmVhZCIsIndhbGxldDphY3Rpb24iLCJ3YWxsZXQ6cmVhZCJdLCJpc18yZmFfZGlzYWJsZWQiOmZhbHNlLCJuYW1lIjoiTWF4QXBpVG9rZW4iLCJpc19kaXNhYmxlX29ubGluZSI6dHJ1ZX0.K0OxFBt4SgrAGCNlGrAQ2krbBtfr1eM45Ph_MsMcuOEzRu1fZHCCL9O59EpdMzHkU72pj3E8G9tWiTPblZFsEw"));
		}
		private static void AspConfigurationAndRun(string[] args)
		{
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/ApiProxy.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 3)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
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