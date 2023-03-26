using LABA1Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABA1Client.Tasks
{
    /// <summary>
    /// Класс работы с альбомами
    /// </summary>
    public static class AlbumTasks
    {
        /// <summary>
        /// Получение и вывод списка всех альбомов
        /// </summary>
        /// <returns>Task</returns>
        public static async Task WriteAlbumsAsync()
        {
            try
            {
                using var client = new HttpClient();

                // Отправляем GET-запрос к API и получаем JSON-строку в ответ
                HttpResponseMessage response = await client.GetAsync("https://localhost:7245/Albums");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Album> albums = JsonConvert.DeserializeObject<List<Album>>(responseBody);
                if (albums == null)
                {
                    Console.WriteLine("\nАльбомы не найдены.\n");
                    return;
                }
                if (albums.Count < 1)
                {
                    Console.WriteLine("\nАльбомы не найдены.\n");
                    return;
                }
                // Определяем максимальную длину полей в объекте Album
                int maxAlbumLength = albums.Max(x => x.album.Length);
                int maxSongsLength = albums.Max(x => x.songs.Length);
                int maxCountLength = 20;
                int maxArtistLength = albums.Max(x => x.artist.Length);
                int maxClientLength = albums.Max(x => x.client.Length);

                // Создаем форматированный вывод таблицы
                string format = "| {0,-" + maxAlbumLength + "} | {1,-" + maxSongsLength + "} | {2,-" + maxCountLength + "} | {3,-" + maxArtistLength + "} | {4,-" + maxClientLength + "} |";
                int widthTable = maxAlbumLength + maxSongsLength + maxCountLength + maxArtistLength + maxClientLength;
                // Выводим заголовок таблицы
                Console.WriteLine(new string('-', widthTable + 16));
                Console.WriteLine(format, "Альбом", "Песни", "Количество заказов", "Артисты", "Клиенты");
                Console.WriteLine(new string('-', widthTable + 16));

                // Выводим каждый альбом в таблице
                foreach (Album album in albums)
                {
                    Console.WriteLine(format, album.album, album.songs, album.countAlbum, album.artist, album.client);
                    Console.WriteLine(new string('-', widthTable + 16));
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
