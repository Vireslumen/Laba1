using Laba1API.Data_Access_Layer;
using Laba1API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Laba1API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlbumsController : ControllerBase
    {
        // Инициализируем репозиторий.
        IUserRepository repository = new UserRepository(@"Data Source=.\SQLEXPRESS;Database=LABA1;Integrated Security=SSPI");

        /// <summary>
        /// Получение информации о всех альбомах
        /// </summary>
        /// <returns>Список всех альбомов в JSON</returns>
        [HttpGet]
        public string Get()
        {
            try
            {
                List<Album> albums = repository.GetAlbums();
                // Сериализуем объекты в JSON.
                var json = JsonSerializer.Serialize(albums);
                return json.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
