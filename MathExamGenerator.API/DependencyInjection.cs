using System.Text;
using CloudinaryDotNet;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Settings;
using MathExamGenerator.Repository.Implement;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Implement;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace MathExamGenerator.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork<MathExamGeneratorContext>, UnitOfWork<MathExamGeneratorContext>>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<MathExamGeneratorContext>(options => options.UseSqlServer(GetConnectionString()));
            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IExamExchangeService, ExamExchangeService>();
            services.AddScoped<IBookTopicService, BookTopicService>();
            services.AddScoped<IBookChapterService, BookChapterService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ISubjectBookService, SubjectBookService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ILikeCommentService, LikeCommentService>();
            services.AddScoped<IReplyService, ReplyService>();
            return services;
        }
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddLazyResolution(this IServiceCollection services)
        {
            services.AddTransient(typeof(Lazy<>), typeof(LazyResolver<>));
            return services;
        }

        private class LazyResolver<T> : Lazy<T> where T : class
        {
            public LazyResolver(IServiceProvider serviceProvider)
                : base(() => serviceProvider.GetRequiredService<T>())
            {
            }
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConn = configuration.GetConnectionString("Redis")
                ?? throw new InvalidOperationException("Redis connection string not found");                
            services.AddSingleton<IConnectionMultiplexer>(
                _ => ConnectionMultiplexer.Connect(redisConn));               
            return services;
        }

        public static IServiceCollection AddJwtValidation(this IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = "MATHGENERATOR",
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Convert.FromHexString("0102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F00"))
                };
            });
            return services;
        }

        private static string GetConnectionString()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            var strConn = config["ConnectionStrings:DefautDB"];

            return strConn;
        }

        public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
        {
            var account = new CloudinaryDotNet.Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:Secret"]);

            var cloudinary = new Cloudinary(account)
            {
                Api = { Secure = true }
            };

            services.AddSingleton(account);
            services.AddSingleton(cloudinary);
            return services;
        }

        public static IServiceCollection AddGoogleDrive(this IServiceCollection services, IConfiguration configuration)
        {
            var credentialsSection = configuration.GetSection("GoogleDrive:CredentialsPath");
            var credentialsDict = credentialsSection.GetChildren().ToDictionary(x => x.Key, x => x.Value);
            var credentialsJson = Newtonsoft.Json.JsonConvert.SerializeObject(credentialsDict);

            var folderId = configuration["GoogleDrive:FolderId"];

            var credential = GoogleCredential
                .FromStream(new MemoryStream(Encoding.UTF8.GetBytes(credentialsJson)))
                .CreateScoped(DriveService.ScopeConstants.DriveFile);

            var driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "MathExamGenerator"
            });

            services.AddSingleton(driveService);

            services.Configure<GoogleDriveSettings>(options =>
            {
                options.CredentialsJson = credentialsJson;
                options.FolderId = folderId;
            });

            return services;
        }

    }
}
