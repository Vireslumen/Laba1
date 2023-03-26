using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABA1Client.Tasks
{
    /// <summary>
    /// Класс работы с авторизацией
    /// </summary>
    public static class AutorizationTasks
    {
        // Объявляем переменную для хранения токена
        public static string token = "";

        /// <summary>
        /// Получение и сохранение токена, через авторизацию
        /// </summary>
        /// <returns>Task</returns>
        public static async Task AutorizationAsync()
        {
            try
            {
                Console.Write("Введите логин для авторизации: ");
                string login = Console.ReadLine();
                Console.Write("Введите пароль для авторизации: ");
                string password = Console.ReadLine();

                var httpClient = new HttpClient();
                var requestUri = "https://localhost:7245/api/Login/login";

                var requestData = new
                {
                    username = login,
                    password = password
                };

                var json = System.Text.Json.JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(requestUri, content);

                // Обработка ответа
                if (response.IsSuccessStatusCode)
                {
                    JObject jsontoken = JObject.Parse(await response.Content.ReadAsStringAsync());
                    token = (string)jsontoken["token"];
                }
                else
                {
                    Console.WriteLine($"Ошибка при авторизации.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при отправке запроса: {e.Message}");
            }
        }
    }
}
