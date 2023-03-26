using Laba1API.Data_Access_Layer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


// ������� ������ builder
var builder = WebApplication.CreateBuilder(args);
// ��������� ����������� � �������
builder.Services.AddControllers();
// ��������� � ������� ����������� API Explorer ��� �������� �����
builder.Services.AddEndpointsApiExplorer();
// ��������� �������������� ����� JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    // ����������� ��������� �������� ������
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
// ��������� Swagger ��� ���������������� API
builder.Services.AddSwaggerGen(option =>
{
    // ��������� ���������� � ������������ API
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Laba1 API", Version = "v1" });
    // ��������� �������� ����������� ����� ����� � Swagger UI
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    // ��������� �������� ���������� � ����������� ����� ����� � Swagger UI
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

// �������� ������ ����������
var app = builder.Build();

// ���� �� ��������� � ������ ����������
if (app.Environment.IsDevelopment())
{
    // ���������� Swagger � SwaggerUI
    app.UseSwagger();
    app.UseSwaggerUI();
}
// �������������� HTTP ������� �� HTTPS
app.UseHttpsRedirection();
// ��������� ������������� �����������
app.UseAuthorization();
// ��������� ������� ��� ������������
app.MapControllers();
// ��������� ����������
app.Run();
