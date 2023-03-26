using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
    public class AddOrderController : ControllerBase
    {
        // Инициализируем репозиторий.
        IUserRepository repository = new UserRepository(@"Data Source=.\SQLEXPRESS;Database=LABA1;Integrated Security=SSPI");

        /// <summary>
        /// Добавление заказа и получение списка заказов после добавления
        /// </summary>
        /// <param name="email">Email клиента</param>
        /// <param name="album">Название заказываемого альбома</param>
        /// <param name="date">Дата заказа</param>
        /// <returns>Список всех клиентов в JSON</returns>
        [HttpGet]
        public string Get(string email, string album, DateTime date)
        {
            List<Order> clients = repository.AddOrder(email, album, date);
            // Сериализуем объекты в JSON.
            var json = JsonSerializer.Serialize(clients);
            return json.ToString();
        }
    }
}
