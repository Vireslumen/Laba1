using Laba1API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Laba1API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// Авторизация и получение токена
        /// </summary>
        /// <param name="loginDTO">Объект класса LoginDTO с username и password</param>
        /// <returns>Успешность атворизации</returns>
        [HttpPost, Route("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            try
            {
                if (loginDTO == null)
                {
                    return BadRequest("Invalid client request");
                }
                if (loginDTO.UserName == "login" && loginDTO.Password == "password")
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyKeySecret"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenOptions = new JwtSecurityToken(
                        issuer: "Stankin",
                        audience: "https://localhost:7245",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: signinCredentials
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    return Ok(new { Token = tokenString });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch
            {
                return Unauthorized();
            }
        }
    }
}
