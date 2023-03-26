using Laba1API.Data_Access_Layer;
using Laba1API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Laba1API.Controllers
{
    [ApiController]
    [Route("[controller]"), Authorize]
    public class AddClientController : ControllerBase
    {
        // Инициализируем репозиторий.
        IUserRepository repository = new UserRepository(@"Data Source=.\SQLEXPRESS;Database=LABA1;Integrated Security=SSPI");

        /// <summary>
        /// Добавление клиента и получение списка клиентов после добавления
        /// </summary>
        /// <param name="name">Имя клиента</param>
        /// <param name="email">Email клиента</param>
        /// <param name="phone">Телефон клиента</param>
        /// <param name="address">Адресс клиента</param>
        /// <returns>Список всех клиентов в JSON</returns>
        [HttpGet]
        public string Get(string name, string email, string phone, string address)
        {
            List<Client> clients = repository.AddClient(name, email, phone, address);
            // Сериализуем объекты в JSON.
            var json = JsonSerializer.Serialize(clients);
            return json.ToString();
        }
    }
}
