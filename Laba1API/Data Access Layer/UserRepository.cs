using Dapper;
using Laba1API.Models;
using System.Data;
using System.Data.SqlClient;

namespace Laba1API.Data_Access_Layer
{
    /// <summary>
    /// Интерфейс для репозитория пользователей, включающий ряд методов
    /// </summary>
    public interface IUserRepository
    {
        List<Song> GetSongsByEmail(string email);
        List<Client> GetClientsBySong(string song);
        List<Album> GetAlbums();
        List<Order> AddOrder(string email, string album, DateTime date);
        List<Client> AddClient(string name, string email, string phone, string address);
    }

    /// <summary>
    /// Реализация интерфейса IUserRepository
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Поле для хранения объекта подключения к базе данных
        /// </summary>
        private readonly IDbConnection _dbConnection;

        /// <summary>
        /// Конструктор, создающий новый объект UserRepository с заданным connectionString
        /// </summary>
        /// <param name="connectionString">Строка поключения к БД</param>
        public UserRepository(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Получить список песен по email пользователя
        /// </summary>
        /// <param name="email">Email клиента</param>
        /// <returns>Список клиентов</returns>
        public List<Song> GetSongsByEmail(string email)
        {
            return _dbConnection.Query<Song>($"SELECT s.title AS song_title, al.title AS album_title, ar.name AS artist_name, s.duration AS song_duration\r\nFROM Clients c\r\nINNER JOIN Orders o ON c.client_id = o.client_id\r\nINNER JOIN Albums al ON o.album_id = al.album_id\r\nINNER JOIN Songs s ON al.album_id = s.album_id\r\nINNER JOIN Artists ar ON s.artist_id = ar.artist_id\r\nWHERE c.email = '{email}'\r\nGROUP BY s.title, al.title, ar.name, s.duration;").ToList();
        }

        /// <summary>
        /// Получить список клиентов, заказавших определенную песню
        /// </summary>
        /// <param name="song">Заказанная песня</param>
        /// <returns>Список клиентов</returns>
        public List<Client> GetClientsBySong(string song)
        {
            return _dbConnection.Query<Client>($"SELECT c.name, c.email, c.phone, c.address FROM Clients c JOIN Orders o ON c.client_id = o.client_id JOIN Albums a ON o.album_id = a.album_id JOIN Songs s ON a.album_id = s.album_id WHERE s.title = '{song}';").ToList();
        }

        /// <summary>
        /// Добавить клиента в базу данных
        /// </summary>
        /// <param name="name">Имя клиента</param>
        /// <param name="email">Email клиента</param>
        /// <param name="phone">Телефон клиента</param>
        /// <param name="address">Адресс клиента</param>
        /// <returns>Список клиентов после добавления</returns>
        public List<Client> AddClient(string name, string email, string phone, string address)
        {
            
            return _dbConnection.Query<Client>($"INSERT INTO Clients (name, email, phone, address) VALUES ( '{name}', '{email}', '{phone}', '{address}') SELECT c.name, c.email, c.phone, c.address FROM Clients c").ToList();
        }

        /// <summary>
        /// Получить список всех альбомов
        /// </summary>
        /// <returns>Список альбомов</returns>
        public List<Album> GetAlbums()
        {
          return _dbConnection.Query<Album>("SELECT al.title as album, STUFF((SELECT distinct ', ' + s.title FROM Songs s WHERE s.album_id = al.album_id FOR XML PATH('')), 1, 1, '') AS songs, COUNT(DISTINCT o.order_id) as countAlbum, ar.name as artist, CASE WHEN COUNT(DISTINCT o.client_id) = 0 THEN '' ELSE (SELECT TOP 1 c.name FROM Orders o2 INNER JOIN Clients c on o2.client_id = c.client_id WHERE o2.album_id=al.album_id GROUP BY o2.client_id, c.name ORDER BY COUNT(DISTINCT o2.order_id) DESC) END AS client FROM Albums al INNER JOIN Artists ar ON ar.artist_id = al.artist_id LEFT JOIN Orders o ON o.album_id = al.album_id GROUP BY al.album_id, al.title, ar.name ORDER BY countAlbum DESC; ").ToList();
        }

        /// <summary>
        /// Добавить заказ
        /// </summary>
        /// <param name="email">Email клиента</param>
        /// <param name="album">Название заказываемого альбома</param>
        /// <param name="date">Дата заказа</param>
        /// <returns>Список заказов после добавления</returns>
        public List<Order> AddOrder(string email, string album, DateTime date)
        {
            return _dbConnection.Query<Order>($"INSERT INTO Orders (client_id, album_id, date) SELECT Clients.client_id, Albums.album_id, '{date.ToShortDateString()}' FROM Clients, Albums WHERE Clients.email = '{email}' AND Albums.title = '{album}'; SELECT Clients.email, Albums.title, date FROM Orders JOIN Clients ON Orders.client_id = Clients.client_id JOIN Albums ON Orders.album_id = Albums.album_id;").ToList();
        }

    }
}
