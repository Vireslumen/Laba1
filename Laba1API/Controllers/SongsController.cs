using Laba1API.Data_Access_Layer;
using Laba1API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Laba1API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SongsController : ControllerBase
    {
        // �������������� �����������.
        IUserRepository repository = new UserRepository(@"Data Source=.\SQLEXPRESS;Database=LABA1;Integrated Security=SSPI");

        /// <summary>
        /// ��������� �����, ������� �������� ������ � ������������ email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>������ ����� � JSON</returns>
        [HttpGet]
        public string Get(string email)
        {
            try
            {
                List<Song> songs = repository.GetSongsByEmail(email);
                // ����������� ������� � JSON.
                var json = JsonSerializer.Serialize(songs);
                return json.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }


}