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
            int count = 0;
            using (var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    // Odczytaj pierwszą (i jedyną) kolumnę (wynik COUNT(*))
                    // Użyj GetInt32 i sprawdź IsDBNull
                    if (!reader.IsDBNull(0))
                    {
                        count = reader.GetInt32(0);
                    }
                }
            }
            return count;
        }

        public async Task<int> GetConfirmedReservationsCountAsync()
        {
            var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(
                "SELECT COUNT(*) FROM rezerwacje WHERE status = 'zrealizowana'", conn);
            int count = 0;
            using (var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    // Odczytaj pierwszą (i jedyną) kolumnę (wynik COUNT(*))
                    if (!reader.IsDBNull(0))
                    {
                        count = reader.GetInt32(0);
                    }
                }
            }
            return count;
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

        public async Task<Room> GetRoomByIdAsync(int roomId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var cmd = new MySqlCommand("SELECT * FROM pokoje WHERE pokoj_id = @roomId", conn);
                cmd.Parameters.AddWithValue("@roomId", roomId);

                using (var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Room
                        {
                            PokojId = reader.GetInt32("pokoj_id"),
                            Nazwa = reader.GetString("nazwa"),
                            Opis = reader.GetString("opis"),
                            Trudnosc = reader.GetByte("trudnosc"),
                            Cena = reader.GetDecimal("cena"),
                            MaxGraczy = reader.GetByte("max_graczy"),
                            CzasMinut = reader.GetInt32("czas_minut")
                        };
                    }
                }
            }
            return null;
        }

        public string GetConnectionString()
        {
            return connectionString;
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime date)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    // Sprawdź, czy istnieją aktywne rezerwacje dla tego pokoju na tę datę
                    var cmd = new MySqlCommand(
                        @"SELECT COUNT(*) FROM rezerwacje 
                WHERE pokoj_id = @roomId 
                AND DATE(data_rozpoczecia) = DATE(@date) 
                AND status = 'zarezerwowana'", conn);

                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    cmd.Parameters.AddWithValue("@date", date.Date);

                    int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                    // Logowanie do debugowania
                    System.Diagnostics.Debug.WriteLine($"Znaleziono {count} rezerwacji dla pokoju {roomId} na datę {date.Date}");

                    // Jeśli istnieją rezerwacje na ten dzień, pokój jest niedostępny
                    return count == 0;
                }
            }
            catch (Exception ex)
            {
                // Logowanie błędu do debugowania
                System.Diagnostics.Debug.WriteLine($"Błąd w IsRoomAvailableAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                // W przypadku błędu zakładamy, że pokój jest niedostępny
                return false;
            }
        }

        public async Task<string> GetRoomStatusAsync(int roomId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var cmd = new MySqlCommand("SELECT status_pokoj FROM pokoje WHERE pokoj_id = @roomId", conn);
                cmd.Parameters.AddWithValue("@roomId", roomId);

                var result = await cmd.ExecuteScalarAsync();
                return result?.ToString() ?? "wolny";
            }
        }

        public async Task UpdateRoomStatusAsync(int roomId, string status)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var cmd = new MySqlCommand("UPDATE pokoje SET status_pokoj = @status WHERE pokoj_id = @roomId", conn);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@roomId", roomId);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> AddReservationAsync(Reservation reservation)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();

                try
                {
                    var cmd = new MySqlCommand(
                        @"INSERT INTO rezerwacje 
                  (uzytkownik_id, pokoj_id, data_rozpoczecia, liczba_osob, status, data_utworzenia) 
                  VALUES 
                  (@uzytkownikId, @pokojId, @dataRozpoczecia, @liczbaOsob, @status, @dataUtworzenia)", conn);

                    cmd.Parameters.AddWithValue("@uzytkownikId", reservation.UzytkownikId);
                    cmd.Parameters.AddWithValue("@pokojId", reservation.PokojId);
                    cmd.Parameters.AddWithValue("@dataRozpoczecia", reservation.DataRozpoczecia);
                    cmd.Parameters.AddWithValue("@liczbaOsob", reservation.LiczbaOsob);
                    cmd.Parameters.AddWithValue("@status", reservation.Status.ToString());
                    cmd.Parameters.AddWithValue("@dataUtworzenia", reservation.DataUtworzenia);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        // Pobierz ID nowo utworzonej rezerwacji
                        cmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn);
                        reservation.RezerwacjaId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        // Aktualizuj status pokoju na "zarezerwowany"
                        await UpdateRoomStatusAsync(reservation.PokojId, "zarezerwowany");

                        return true;
                    }

                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<bool> UpdateReservationStatusAsync(int reservationId, ReservationStatus newStatus)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();
                try
                {
                    var cmd = new MySqlCommand(
                        "UPDATE rezerwacje SET status = @status WHERE rezerwacja_id = @reservationId", conn);
                    cmd.Parameters.AddWithValue("@status", newStatus.ToString());
                    cmd.Parameters.AddWithValue("@reservationId", reservationId);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Błąd podczas aktualizacji statusu rezerwacji {reservationId} na {newStatus}: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    return false;
                }
            }
        }

        public async Task<bool> AddPaymentAsync(int reservationId, decimal amount, DateTime paymentDate)
        {
          
            await Task.Delay(100); // Symuluj asynchroniczną operację
            System.Diagnostics.Debug.WriteLine($"Symulacja dodawania płatności: Rezerwacja ID: {reservationId}, Kwota: {amount:C}, Data: {paymentDate}");
            return true;
        }

        public async Task<List<Message>> GetRecentMessagesAsync(int limit)
        {
            var messages = new List<Message>();
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var cmd = new MySqlCommand("SELECT w.id_wiadomosci, w.wiadomosc, w.uzytkownik_id, u.email FROM wiadomosci as w JOIN uzytkownicy as u ON w.uzytkownik_id = u.uzytkownik_id ORDER BY id_wiadomosci DESC LIMIT @limit", conn);
                cmd.Parameters.AddWithValue("@limit", limit);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        messages.Add(new Message
                        {
                            WiadomoscId = reader.GetInt32(reader.GetOrdinal("id_wiadomosci")),
                            NadawcaId = reader.GetInt32(reader.GetOrdinal("uzytkownik_id")),
                            Tresc = reader.GetString(reader.GetOrdinal("wiadomosc")),
                            Email = reader.GetString(reader.GetOrdinal("email"))

                        });
                    }
                }
            }
            return messages;
        }

        public async Task<int> ApproveAllPendingReservationsAsync()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var cmd = new MySqlCommand(
                    "UPDATE rezerwacje SET status = 'zrealizowana' WHERE status = 'zarezerwowana'", conn);

                int affectedRows = await cmd.ExecuteNonQueryAsync();
                return affectedRows; // liczba zaktualizowanych rezerwacji
            }
        }

        public async Task<bool> SaveContactMessageAsync(string message, int? userId)
        {
            using (var conn = new MySqlConnection(connectionString))
            try
            {
                await conn.OpenAsync();
                var cmd = new MySqlCommand("INSERT INTO wiadomosci (wiadomosc, uzytkownik_id) VALUES (@message, @userId)", conn);
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters.AddWithValue("@userId", userId.HasValue ? userId.Value : (object)DBNull.Value);
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving message: " + ex.Message);
                return false;
            }
        }

    }
}
