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

            MySqlCommand cmd = new MySqlCommand("SELECT pokoj_id, nazwa, opis, trudnosc, cena, max_graczy, czas_minut FROM Pokoje", conn);
            MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Room r = new Room
                {
                    PokojId = reader.GetInt32("pokoj_id"),
                    Nazwa = reader.GetString("nazwa"),
                    Opis = reader.GetString("opis"),
                    Trudnosc = reader.GetByte("trudnosc"),
                    Cena = reader.GetDecimal("cena"),
                    MaxGraczy = reader.GetByte("max_graczy"),
                    CzasMinut = reader.GetInt32("czas_minut")
                };

                rooms.Add(r);
            }

            await conn.CloseAsync();

            return rooms;
        }

        public async Task<User> GetUserByEmailAsync(string email)
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
                    Telefon = reader.IsDBNull(reader.GetOrdinal("telefon")) ? null : reader.GetString("telefon"),
                    Email = reader.GetString("email"),
                    HasloHash = reader.GetString("haslo_hash"),
                    Admin = reader.GetBoolean("admin")
                };
            }
            await conn.CloseAsync();
            return user;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO uzytkownicy (email, haslo_hash, imie, nazwisko, telefon, data_rejestracji, admin) " +
                    "VALUES (@email, @haslo_hash, @imie, @nazwisko, @telefon, @data_rejestracji, @admin)", conn);

                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@haslo_hash", user.HasloHash);
                cmd.Parameters.AddWithValue("@imie", user.Imie);
                cmd.Parameters.AddWithValue("@nazwisko", user.Nazwisko);
                cmd.Parameters.AddWithValue("@telefon", user.Telefon ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@data_rejestracji", user.DataRejestracji);
                cmd.Parameters.AddWithValue("@admin", user.Admin);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        public async Task<User> GetUserByPhoneAsync(string phone)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();
            MySqlCommand cmd = new MySqlCommand("SELECT uzytkownik_id, email, haslo_hash, imie, nazwisko, telefon, admin FROM uzytkownicy WHERE telefon = @telefon", conn);
            cmd.Parameters.AddWithValue("@telefon", phone);
            MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            User user = null;
            if (await reader.ReadAsync())
            {
                user = new User
                {
                    UzytkownikId = reader.GetInt32("uzytkownik_id"),
                    Imie = reader.GetString("imie"),
                    Nazwisko = reader.GetString("nazwisko"),
                    Telefon = reader.IsDBNull(reader.GetOrdinal("telefon")) ? null : reader.GetString("telefon"),
                    Email = reader.GetString("email"),
                    HasloHash = reader.GetString("haslo_hash"),
                    Admin = reader.GetBoolean("admin")
                };
            }
            await conn.CloseAsync();
            return user;
        }

        public async Task<List<Reservation>> GetRecentReservationsAsync(int limit)
        {
            var reservations = new List<Reservation>();
            var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(
                "SELECT rezerwacja_id, status, liczba_osob FROM rezerwacje ORDER BY rezerwacja_id DESC LIMIT @limit", conn);
            cmd.Parameters.AddWithValue("@limit", limit);

            var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                reservations.Add(new Reservation
                {
                    RezerwacjaId = reader.GetInt32("rezerwacja_id"),
                    Status = (ReservationStatus)Enum.Parse(typeof(ReservationStatus), reader.GetString("status")),
                    LiczbaOsob = reader.GetByte("liczba_osob")
                });
            }

            await conn.CloseAsync();
            return reservations;
        }

        public async Task<List<User>> GetRecentUsersAsync(int limit)
        {
            var users = new List<User>();
            var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(
                "SELECT uzytkownik_id, imie, nazwisko FROM uzytkownicy ORDER BY data_rejestracji DESC LIMIT @limit", conn);
            cmd.Parameters.AddWithValue("@limit", limit);

            var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    UzytkownikId = reader.GetInt32("uzytkownik_id"),
                    Imie = reader.GetString("imie"),
                    Nazwisko = reader.GetString("nazwisko")
                });
            }

            await conn.CloseAsync();
            return users;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT COUNT(*) FROM uzytkownicy", conn);
            var result = await cmd.ExecuteScalarAsync();

            await conn.CloseAsync();
            return Convert.ToInt32(result);
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(
                "SELECT SUM(pokoje.cena) FROM pokoje JOIN rezerwacje ON pokoje.pokoj_id = rezerwacje.pokoj_id WHERE rezerwacje.status = \"zrealizowana\"", conn);
            var result = await cmd.ExecuteScalarAsync();

            await conn.CloseAsync();
            return Convert.ToDecimal(result);
        }

        public async Task<int> GetPendingReservationsCountAsync()
        {
            var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(
                "SELECT COUNT(*) FROM rezerwacje WHERE status = 'zarezerwowana'", conn);
            var result = await cmd.ExecuteScalarAsync();

            await conn.CloseAsync();
            return Convert.ToInt32(result);
        }

        public async Task<int> GetConfirmedReservationsCountAsync()
        {
            var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(
                "SELECT COUNT(*) FROM rezerwacje WHERE status = 'zrealizowana'", conn);
            var result = await cmd.ExecuteScalarAsync();

            await conn.CloseAsync();
            return Convert.ToInt32(result);
        }

        public async Task<int> GetActiveRoomsCountAsync()
        {
            var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT COUNT(*) FROM Pokoje", conn);
            var result = await cmd.ExecuteScalarAsync();

            await conn.CloseAsync();
            return Convert.ToInt32(result);
        }

    }
}
