using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LABA1Client.Models;

namespace LABA1Client.Tasks
{
    /// <summary>
    /// Класс работы с клиентами
    /// </summary>
    public static class ClientTasks
    {
        /// <summary>
        /// Добавление клиента
        /// </summary>
        /// <returns>Task</returns>
        public static async Task AddClientAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                //Добавление авторизации
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AutorizationTasks.token);

                Console.Write("Введите имя клиента: ");
                string name = Console.ReadLine();
                name = Regex.Replace(name, " ", "%20");
                Console.Write("Введите email клиента: ");
                string email = Console.ReadLine();
                email = Regex.Replace(email, " ", "%20");
                Console.Write("Введите телефон клиента: ");
                string phone = Console.ReadLine();
                phone = Regex.Replace(phone, " ", "%20");
                Console.Write("Введите адресс клиента: ");
                string address = Console.ReadLine();
                address = Regex.Replace(address, " ", "%20");
                // Отправляем GET-запрос к API и получаем JSON-строку в ответ
                HttpResponseMessage response = await client.GetAsync($"https://localhost:7245/AddClient?name={name}&email={email}&phone={phone}&address={address}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Client> clients = JsonConvert.DeserializeObject<List<Client>>(responseBody);
                if (clients == null)
                {
                    Console.WriteLine("\nКлиенты не найдены.\n");
                    return;
                }
                if (clients.Count < 1)
                {
                    Console.WriteLine("\nКлиенты не найдены.\n");
                    return;
                }
                int maxNameLength = clients.Max(x => x.name.Length);
                int maxEmailLength = clients.Max(x => x.email.Length);
                int maxPhoneLength = clients.Max(x => x.phone.Length);
                int maxAddressLength = clients.Max(x => x.address.Length);

                // Создаем форматированный вывод таблицы
                string format = "| {0,-" + maxNameLength + "} | {1,-" + maxEmailLength + "} | {2,-" + maxPhoneLength + "} | {3,-" + maxAddressLength + "} |";
                int widthTable = maxNameLength + maxEmailLength + maxPhoneLength + maxAddressLength;
                // Выводим заголовок таблицы
                Console.WriteLine(new string('-', widthTable + 13));
                Console.WriteLine(format, "Имя", "Email", "Телефон", "Адресс");
                Console.WriteLine(new string('-', widthTable + 13));

                // Выводим каждый альбом в таблице
                foreach (Client cl in clients)
                {
                    Console.WriteLine(format, cl.name, cl.email, cl.phone, cl.address);
                    Console.WriteLine(new string('-', widthTable + 13));
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка при отправке запроса: {e.Message}");
            }
        }

        /// <summary>
        /// Получение и вывод списка клиентов исполнивших указанную песню
        /// </summary>
        /// <returns>Task</returns>
        public static async Task WriteClientsAsync()
        {
            try
            {
                using var client = new HttpClient();
                Console.Write("Введите песню, чтобы получить клиентов, заказавших её: ");
                string song = Console.ReadLine();
                song = Regex.Replace(song, " ", "%20");
                // Отправляем GET-запрос к API и получаем JSON-строку в ответ
                HttpResponseMessage response = await client.GetAsync($"https://localhost:7245/Client?song={song}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Client> clients = JsonConvert.DeserializeObject<List<Client>>(responseBody);
                if (clients == null)
                {
                    Console.WriteLine("\nКлиенты не найдены.\n");
                    return;
                }
                if (clients.Count < 1)
                {
                    Console.WriteLine("\nКлиенты не найдены.\n");
                    return;
                }
                int maxNameLength = clients.Max(x => x.name.Length);
                int maxEmailLength = clients.Max(x => x.email.Length);
                int maxPhoneLength = clients.Max(x => x.phone.Length);
                int maxAddressLength = clients.Max(x => x.address.Length);

                // Создаем форматированный вывод таблицы
                string format = "| {0,-" + maxNameLength + "} | {1,-" + maxEmailLength + "} | {2,-" + maxPhoneLength + "} | {3,-" + maxAddressLength + "} |";
                int widthTable = maxNameLength + maxEmailLength + maxPhoneLength + maxAddressLength;
                // Выводим заголовок таблицы
                Console.WriteLine(new string('-', widthTable + 13));
                Console.WriteLine(format, "Имя", "Email", "Телефон", "Адресс");
                Console.WriteLine(new string('-', widthTable + 13));

                // Выводим каждый альбом в таблице
                foreach (Client cl in clients)
                {
                    Console.WriteLine(format, cl.name, cl.email, cl.phone, cl.address);
                    Console.WriteLine(new string('-', widthTable + 13));
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
