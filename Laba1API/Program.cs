using Laba1API.Data_Access_Layer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


// Создаем объект builder
var builder = WebApplication.CreateBuilder(args);
// Добавляем контроллеры в сервисы
builder.Services.AddControllers();
// Добавляем в сервисы возможность API Explorer для конечных точек
builder.Services.AddEndpointsApiExplorer();
// Добавляем аутентификацию через JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    // Настраиваем параметры проверки токена
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "Stankin",
        ValidAudience = "https://localhost:7245",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyKeySecret"))
    };
});
// Добавляем Swagger для документирования API
builder.Services.AddSwaggerGen(option =>
{
    // Указываем информацию о документации API
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Laba1 API", Version = "v1" });
    // Добавляем описание авторизации через токен в Swagger UI
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    // Добавляем описание требований к авторизации через токен в Swagger UI
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Собираем объект приложения
var app = builder.Build();

// Если мы находимся в режиме разработки
if (app.Environment.IsDevelopment())
{
    // Используем Swagger и SwaggerUI
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Перенаправляем HTTP запросы на HTTPS
app.UseHttpsRedirection();
// Добавляем использование авторизации
app.UseAuthorization();
// Добавляем маршрут для контроллеров
app.MapControllers();
// Запускаем приложение
app.Run();
