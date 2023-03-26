using Laba1API.Data_Access_Layer;
using Laba1API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Laba1API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        // Инициализируем репозиторий.
        IUserRepository repository = new UserRepository(@"Data Source=.\SQLEXPRESS;Database=LABA1;Integrated Security=SSPI");

        /// <summary>
        /// Получаем клиентов, которые исполняли выбранную песню
        /// </summary>
        /// <param name="song">Песня</param>
        /// <returns>Список клиентов в JSON</returns>
        [HttpGet]
        public string Get(string song)
        {
            try
            {
                List<Client> clients = repository.GetClientsBySong(song);
                // Сериализуем объекты в JSON.
                var json = JsonSerializer.Serialize(clients);
                return json.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
