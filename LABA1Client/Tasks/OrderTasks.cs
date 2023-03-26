using LABA1Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LABA1Client.Tasks
{
    /// <summary>
    /// Класс работы с заказами
    /// </summary>
    public static class OrderTasks
    {
        /// <summary>
        /// Добавление заказа
        /// </summary>
        /// <returns>Task</returns>
        public async static Task AddOrderAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                //Добавление авторизации
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AutorizationTasks.token);
                Console.Write("Введите email клиента: ");
                string email = Console.ReadLine();
                Console.Write("Введите название заказанного альбома: ");
                string album = Console.ReadLine();
                album = Regex.Replace(album, " ", "%20");
                Console.Write("Введите дату заказа: ");
                DateTime date = DateTime.Parse(Console.ReadLine());
                // Отправляем GET-запрос к API и получаем JSON-строку в ответ
                HttpResponseMessage response = await client.GetAsync($"https://localhost:7245/AddOrder?email={email}&album={album}&date={date.ToShortDateString()}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(responseBody);
                if (orders == null)
                {
                    Console.WriteLine("\nЗаказы не найдены.\n");
                    return;
                }
                if (orders.Count < 1)
                {
                    Console.WriteLine("\nЗаказы не найдены.\n");
                    return;
                }
                int maxEmailLength = orders.Max(x => x.email.Length);
                int maxTitleLength = orders.Max(x => x.title.Length);
                int maxDateLength = orders.Max(x => x.date.ToLongDateString().Length);

                // Создаем форматированный вывод таблицы
                string format = "| {0,-" + maxEmailLength + "} | {1,-" + maxTitleLength + "} | {2,-" + maxDateLength + "} |";
                int widthTable = maxEmailLength + maxTitleLength + maxDateLength;
                // Выводим заголовок таблицы
                Console.WriteLine(new string('-', widthTable + 10));
                Console.WriteLine(format, "Email", "Альбом", "Дата");
                Console.WriteLine(new string('-', widthTable + 10));

                // Выводим каждый альбом в таблице
                foreach (Order ord in orders)
                {
                    Console.WriteLine(format, ord.email, ord.title, ord.date.ToLongDateString());
                    Console.WriteLine(new string('-', widthTable + 10));
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка при отправке запроса: {e.Message}");
            }
        }
    }
}
