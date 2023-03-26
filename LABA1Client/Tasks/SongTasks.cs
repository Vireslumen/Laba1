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
    /// Класс работы с песнями
    /// </summary>
    public static class SongTasks
    {
        /// <summary>
        /// Получение и вывод списка песен, которые были исполнены клиентов
        /// </summary>
        /// <returns>Task</returns>
        public static async Task WriteSongsAsync()
        {
            try
            {
                using var client = new HttpClient();

                Console.Write("Введите email клиента, чтобы узнать какие песни заказывал клиент: ");
                string email = Console.ReadLine();
                email = Regex.Replace(email, " ", "%20");
                // Отправляем GET-запрос к API и получаем JSON-строку в ответ
                HttpResponseMessage response = await client.GetAsync($"https://localhost:7245/Songs?email={email}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Song> songs = JsonConvert.DeserializeObject<List<Song>>(responseBody);
                if (songs == null)
                {
                    Console.WriteLine("\nПесни не найдены.\n");
                    return;
                }
                if (songs.Count < 1)
                {
                    Console.WriteLine("\nПесни не найдены.\n");
                    return;
                }
                int maxSongTitleLength = songs.Max(x => x.song_title.Length);
                int maxAlbumTitleLength = songs.Max(x => x.album_title.Length);
                int maxArtistNameLength = songs.Max(x => x.artist_name.Length);
                int maxSongDurationLength = 14;

                // Создаем форматированный вывод таблицы
                string format = "| {0,-" + maxSongTitleLength + "} | {1,-" + maxAlbumTitleLength + "} | {2,-" + maxArtistNameLength + "} | {3,-" + maxSongDurationLength + "} |";
                int widthTable = maxSongTitleLength + maxAlbumTitleLength + maxArtistNameLength + maxSongDurationLength;
                // Выводим заголовок таблицы
                Console.WriteLine(new string('-', widthTable + 13));
                Console.WriteLine(format, "Песня", "Альбом", "Артист", "Длительность");
                Console.WriteLine(new string('-', widthTable + 13));

                // Выводим каждый альбом в таблице
                foreach (Song sg in songs)
                {
                    Console.WriteLine(format, sg.song_title, sg.album_title, sg.artist_name, sg.song_duration);
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
