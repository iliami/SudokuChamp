
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.IdentityModel.Tokens;
using SudokuChamp.API.DAL.Json;
using SudokuChamp.API.DAL.Repo.Abstract;
using SudokuChamp.API.DAL.Repo.Json;
using SudokuChamp.Server.Services;
using SudokuChamp.Server.Services.Abstract;
using SudokuChamp.Server.Utils;
using SudokuChamp.Server.Utils.Abstract;

namespace SudokuChamp.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureFileDatabase();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<JsonContext>();

            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection(nameof(JwtOptions)));
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration.GetRequiredSection(
                                nameof(JwtOptions) + ":SecretKey").Value!)
                        )
                };

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["what-is-that"];

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddScoped<IUserRepo, UserRepoJson>();
            builder.Services.AddScoped<IRecordRepo, RecordRepoJson>();
            builder.Services.AddScoped<ISudokuRepo, SudokuRepoJson>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRecordService, RecordService>();
            builder.Services.AddScoped<ISoloGameService, SoloGameService>();
            builder.Services.AddScoped<ISudokuService, SudokuService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();




            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict
            });

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        public static void ConfigureFileDatabase()
        {
            var pathDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SudokuChamp");
            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }

            var pathGames = Path.Combine(pathDir, "games.txt");
            if (!File.Exists(pathGames))
            {
                File.Create(pathGames);
            }

            var pathUsers = Path.Combine(pathDir, "users.txt");
            if (!File.Exists(pathUsers))
            {
                File.Create(pathUsers);
            }

            var pathSudokus = Path.Combine(pathDir, "sudokus.txt");
            if (!File.Exists(pathSudokus))
            {
                File.WriteAllText(pathSudokus, "{\"Id\":\"c17922f1-79c9-43ad-ad9f-f54c5d9080f8\",\"Board\":\"[[7,2,5,6,3,1,9,4,8],[3,6,9,7,8,4,2,1,0],[4,1,8,5,9,2,3,6,7],[2,4,7,8,1,6,5,3,9],[1,5,3,4,2,9,7,8,6],[9,8,6,3,7,5,1,2,4],[5,3,4,1,6,7,8,9,2],[8,7,2,9,4,3,6,5,1],[6,9,1,2,5,8,4,7,3]]\",\"Difficulty\":1,\"TotalAttemps\":0}\r\n{\"Id\":\"85907ba4-6b5f-491e-86e4-2b7b4e124d16\",\"Board\":\"[[6,9,1,7,0,3,8,4,2],[8,7,2,6,4,1,3,0,0],[3,4,5,0,9,8,6,0,0],[1,0,7,9,8,5,0,2,3],[2,3,4,1,6,0,9,0,8],[0,0,0,0,3,2,1,6,0],[4,2,0,0,7,6,5,1,0],[7,5,6,0,0,9,2,3,0],[9,1,3,0,2,0,7,0,0]]\",\"Difficulty\":2,\"TotalAttemps\":0}\r\n{\"Id\":\"871609e0-74ea-4ce7-a624-1d4f95f21d12\",\"Board\":\"[[0,0,0,4,2,6,9,5,1],[0,9,1,0,3,7,2,4,6],[4,2,6,0,9,1,3,8,7],[2,1,5,0,7,0,8,9,4],[6,8,9,1,4,5,0,3,0],[7,3,4,9,8,0,1,6,5],[0,6,0,7,5,9,4,2,0],[8,5,2,3,1,0,0,0,9],[9,4,7,0,6,0,5,1,0]]\",\"Difficulty\":1,\"TotalAttemps\":0}\r\n{\"Id\":\"cec722a4-a7fd-4787-8349-873b7ed9b491\",\"Board\":\"[[2,8,4,0,7,3,1,0,9],[6,5,9,0,8,2,4,7,3],[7,0,0,5,4,9,8,0,0],[9,0,7,4,0,0,3,8,0],[1,3,5,0,2,8,7,6,4],[8,0,6,3,1,7,2,9,0],[0,0,0,7,0,1,0,0,2],[5,7,1,0,0,4,6,3,8],[4,6,0,8,3,5,0,0,7]]\",\"Difficulty\":2,\"TotalAttemps\":0}\r\n{\"Id\":\"88e89186-7839-46df-ba61-846331244881\",\"Board\":\"[[7,8,9,5,0,0,1,4,3],[0,5,2,3,1,4,0,9,8],[4,1,3,0,9,8,2,6,5],[3,4,0,6,0,0,9,8,2],[2,0,0,1,8,3,0,7,0],[8,7,6,0,4,9,0,0,0],[5,0,8,9,3,7,4,1,6],[9,6,4,8,5,1,0,2,0],[1,3,7,4,2,6,8,5,0]]\",\"Difficulty\":1,\"TotalAttemps\":0}\r\n{\"Id\":\"0ed259a8-6238-4ae7-b995-cdddf065f4fb\",\"Board\":\"[[2,5,0,0,7,6,0,3,0],[0,7,1,0,3,2,6,9,5],[9,0,0,0,5,1,8,7,0],[0,4,0,0,0,5,3,8,0],[5,6,2,3,0,8,7,1,4],[3,8,0,1,4,7,2,5,6],[6,0,5,0,0,0,1,2,3],[0,2,4,5,1,0,9,6,7],[7,1,0,2,6,9,5,0,8]]\",\"Difficulty\":2,\"TotalAttemps\":0}\r\n{\"Id\":\"9924731d-ecc2-4a27-ab98-c8bbb561c3ac\",\"Board\":\"[[0,7,5,4,0,9,1,3,8],[0,8,4,0,1,7,0,0,9],[9,1,6,0,8,5,2,7,4],[4,5,0,1,2,6,8,0,7],[7,2,1,9,4,0,5,0,0],[8,0,9,0,5,3,0,0,2],[0,9,2,6,3,4,7,8,5],[0,0,8,5,7,2,0,4,1],[5,4,7,8,9,1,3,2,6]]\",\"Difficulty\":1,\"TotalAttemps\":0}\r\n{\"Id\":\"b21dbd12-324d-4e20-b10d-7d913f7397c4\",\"Board\":\"[[0,0,0,0,4,0,0,2,0],[0,0,3,5,6,0,1,7,0],[0,0,2,3,0,8,5,6,4],[5,3,0,1,0,9,7,4,0],[4,8,7,6,0,5,9,0,2],[1,0,0,8,7,4,0,0,0],[0,0,4,0,8,3,0,5,1],[0,7,8,0,5,1,4,9,6],[0,0,0,4,0,6,3,8,0]]\",\"Difficulty\":3,\"TotalAttemps\":0}\r\n{\"Id\":\"66f6fc3c-1cfa-4717-b60d-3d5a04dd4bec\",\"Board\":\"[[6,0,3,9,4,8,7,2,1],[2,8,0,0,1,7,3,9,0],[9,7,1,0,2,0,0,5,8],[0,4,0,1,6,5,9,0,2],[7,6,2,0,8,9,5,1,3],[5,1,0,7,3,0,6,0,4],[0,9,6,0,0,4,0,3,5],[1,0,7,6,5,3,8,4,9],[4,3,5,8,9,1,0,6,7]]\",\"Difficulty\":1,\"TotalAttemps\":0}\r\n{\"Id\":\"f89b0748-cdf5-4047-bfad-1e9e063709f5\",\"Board\":\"[[1,0,4,0,7,2,5,8,3],[3,2,9,1,0,0,0,4,6],[0,7,5,4,6,3,1,2,9],[2,4,7,3,9,1,0,6,5],[9,0,6,2,4,0,3,7,1],[5,1,3,7,8,6,2,9,0],[0,0,0,6,1,0,9,5,2],[0,5,1,8,0,9,4,0,7],[7,9,2,0,0,4,6,0,8]]\",\"Difficulty\":1,\"TotalAttemps\":0}\r\n{\"Id\":\"7d8cae54-455f-4342-92b8-baeb68791945\",\"Board\":\"[[5,0,0,0,7,1,3,4,9],[6,9,0,5,8,4,1,7,0],[0,7,1,3,2,9,8,6,5],[9,0,0,0,1,5,6,0,3],[2,1,6,0,3,7,0,0,8],[8,0,5,9,0,0,7,1,4],[0,6,9,2,0,3,4,8,1],[1,0,2,7,4,8,9,3,6],[3,8,0,1,9,6,2,5,7]]\",\"Difficulty\":1,\"TotalAttemps\":0}\r\n{\"Id\":\"b488c67c-80b9-4cdd-bbb0-25d572e4206b\",\"Board\":\"[[0,0,0,8,4,7,3,9,6],[6,7,8,1,3,9,2,4,0],[9,0,3,6,2,5,8,1,7],[4,3,2,9,0,6,0,5,0],[8,9,6,7,5,0,1,3,2],[0,0,7,3,8,0,4,6,9],[7,6,0,2,9,3,5,0,0],[0,0,0,5,7,8,6,2,0],[2,0,5,4,6,1,9,7,3]]\",\"Difficulty\":1,\"TotalAttemps\":0}\r\n{\"Id\":\"e5723468-4492-40ed-b7a4-692a9321fd7e\",\"Board\":\"[[1,8,6,9,0,0,5,0,0],[0,0,7,2,3,0,1,8,0],[5,3,0,8,6,0,0,0,0],[6,2,8,7,4,9,0,5,1],[0,0,0,0,1,0,8,6,0],[0,5,1,0,8,0,2,0,7],[0,0,4,0,0,6,0,1,8],[3,0,9,1,0,8,7,4,0],[8,1,5,0,9,0,6,3,0]]\",\"Difficulty\":3,\"TotalAttemps\":0}\r\n{\"Id\":\"f83b876c-8bff-47d3-babb-273327cc2921\",\"Board\":\"[[5,0,2,0,0,3,6,0,0],[1,0,8,4,0,0,0,2,7],[9,7,0,0,5,0,8,4,0],[4,8,0,9,1,7,0,3,0],[2,0,7,3,8,5,0,9,1],[0,9,0,2,4,6,7,8,0],[7,0,9,0,2,4,3,0,0],[0,2,0,7,0,8,0,5,4],[0,0,0,0,3,1,0,0,2]]\",\"Difficulty\":3,\"TotalAttemps\":0}\r\n{\"Id\":\"685cacdd-050e-46bb-ae31-59a9be6b6273\",\"Board\":\"[[1,7,6,0,9,4,2,5,8],[4,9,0,1,8,2,6,3,7],[3,2,0,6,7,5,9,1,4],[8,5,0,0,0,0,0,4,0],[0,1,4,0,5,9,8,6,3],[6,3,9,8,4,0,0,7,2],[7,8,2,5,1,3,4,0,6],[9,6,1,0,2,0,0,8,0],[5,4,3,9,6,8,0,2,0]]\",\"Difficulty\":1,\"TotalAttemps\":0}\r\n{\"Id\":\"e862b57e-770f-45e8-b3de-6908bbbcfa17\",\"Board\":\"[[7,6,9,5,2,0,8,1,3],[3,4,8,1,0,0,2,7,5],[0,0,2,3,8,0,6,4,9],[5,0,3,0,0,8,9,2,6],[9,8,4,6,0,2,7,3,0],[0,0,0,9,0,0,5,0,4],[4,2,0,8,0,5,3,9,7],[8,3,5,4,0,0,0,6,2],[6,9,7,2,0,0,0,5,8]]\",\"Difficulty\":2,\"TotalAttemps\":0}\r\n{\"Id\":\"61f8edd6-2f41-4422-b156-e3ef469a4728\",\"Board\":\"[[6,0,3,0,1,2,7,8,0],[5,7,0,0,0,8,0,0,0],[4,8,1,7,3,0,0,0,5],[0,0,0,6,5,0,0,4,0],[9,6,4,0,8,1,5,0,7],[8,0,5,4,2,0,0,6,3],[7,4,8,0,0,3,0,5,1],[0,5,6,1,0,4,8,0,0],[1,0,0,8,0,5,0,0,2]]\",\"Difficulty\":3,\"TotalAttemps\":0}\r\n{\"Id\":\"9567f75a-3f79-45e2-864b-368fb94d04d2\",\"Board\":\"[[1,3,0,0,9,5,6,7,0],[9,5,0,7,0,3,1,0,0],[7,0,2,0,8,0,9,3,5],[0,8,3,6,0,0,0,0,0],[0,6,0,2,3,9,8,0,0],[0,0,0,8,0,4,2,6,3],[0,1,0,0,7,0,3,4,6],[6,2,0,0,4,1,0,0,8],[3,9,0,0,0,8,7,2,1]]\",\"Difficulty\":3,\"TotalAttemps\":0}\r\n{\"Id\":\"8e1feae6-ca41-459f-9061-c5ca1e1d4c00\",\"Board\":\"[[0,5,3,0,0,7,2,1,9],[1,6,9,2,4,5,3,7,8],[0,0,7,0,9,1,5,4,6],[0,2,0,0,1,6,0,8,7],[6,7,1,9,0,8,4,3,5],[8,9,0,0,0,4,1,0,2],[5,1,0,4,6,0,7,2,3],[7,3,6,1,5,0,8,0,4],[0,4,2,0,0,0,6,0,0]]\",\"Difficulty\":2,\"TotalAttemps\":0}\r\n{\"Id\":\"ddd927c6-76dc-438e-ac82-2bd80a27e6d9\",\"Board\":\"[[0,0,0,2,3,0,6,0,8],[0,6,9,7,8,1,2,5,3],[2,3,8,5,4,6,1,0,7],[0,8,0,6,0,5,9,0,4],[0,4,5,0,1,0,0,0,0],[6,0,7,4,9,2,8,0,5],[8,7,3,9,2,4,5,6,1],[1,2,6,8,5,3,4,7,9],[5,0,4,1,6,7,3,8,2]]\",\"Difficulty\":1,\"TotalAttemps\":0}\r\n{\"Id\":\"076af5e8-2d7b-4182-87e3-df21afe340a2\",\"Board\":\"[[7,0,9,0,2,6,0,5,4],[4,6,3,9,7,5,2,8,1],[2,0,0,8,4,3,9,0,6],[5,7,8,0,9,0,6,1,3],[6,0,0,7,0,1,5,4,8],[1,3,4,5,0,8,0,0,2],[9,4,1,3,5,2,0,6,7],[0,0,0,0,0,7,1,2,9],[8,0,0,6,1,0,4,0,0]]\",\"Difficulty\":2,\"TotalAttemps\":0}\r\n");
            }
        }
    }
}
