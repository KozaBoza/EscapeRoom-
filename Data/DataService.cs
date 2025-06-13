using EscapeRoom.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom.Data
{
    public class DataService
    {
        private string connectionString = "Server=localhost;Database=escaperoom;Uid=poig;Pwd=poig;";
        public async Task<List<Room>> GetRoomsAsync()
        {
            List<Room> rooms = new List<Room>();

            MySqlConnection conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT pokoj_id, nazwa, opis, trudnosc, cena_za_godzine, max_graczy, czas_minut FROM Pokoje", conn);
            MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Room r = new Room
                {
                    PokojId = reader.GetInt32("pokoj_id"),
                    Nazwa = reader.GetString("nazwa"),
                    Opis = reader.GetString("opis"),
                    Trudnosc = reader.GetByte("trudnosc"),
                    Cena = reader.GetDecimal("cena_za_godzine"),
                    MaxGraczy = reader.GetByte("max_graczy"),
                    CzasMinut = reader.GetInt32("czas_minut")
                };

                rooms.Add(r);
            }

            await conn.CloseAsync();

            return rooms;
        }

        public async Task<User> GetUserByUsernameAsync(string email)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();
            MySqlCommand cmd = new MySqlCommand("SELECT uzytkownik_id, email, haslo_hash, imie, nazwisko, telefon, admin FROM uzytkownicy WHERE email = @email", conn);
            cmd.Parameters.AddWithValue("@email", email);
            MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            User user = null;
            if (await reader.ReadAsync())
            {
                user = new User
                {
                    UzytkownikId = reader.GetInt32("uzytkownik_id"),
                    Imie = reader.GetString("imie"),
                    Nazwisko = reader.GetString("nazwisko"),
                    Telefon = reader.GetString("telefon"),
                    Email = reader.GetString("email"),
                    HasloHash = reader.GetString("haslo_hash"),
                    Admin = reader.GetBoolean("admin")
                };
            }
            await conn.CloseAsync();
            return user;
        }
    }
}
