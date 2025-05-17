
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using EscapeRoom.Models;

namespace EscapeRoom.Services
{
    public class ReservationService
    {
        private readonly string _connectionString;

        public ReservationService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            var reservations = new List<Reservation>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT r.*, u.Username, u.FirstName, u.LastName, rm.Name as RoomName
                    FROM Reservations r
                    JOIN Users u ON r.UserId = u.Id
                    JOIN Rooms rm ON r.RoomId = rm.Id
                    ORDER BY r.Date DESC, r.StartTime DESC";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        reservations.Add(MapReservationFromReader(reader));
                    }
                }
            }

            return reservations;
        }

        /// <summary>
        /// Pobiera rezerwacje dla danego użytkownika
        /// </summary>
        public async Task<List<Reservation>> GetReservationsByUserIdAsync(int userId)
        {
            var reservations = new List<Reservation>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT r.*, u.Username, u.FirstName, u.LastName, rm.Name as RoomName
                    FROM Reservations r
                    JOIN Users u ON r.UserId = u.Id
                    JOIN Rooms rm ON r.RoomId = rm.Id
                    WHERE r.UserId = @UserId
                    ORDER BY r.Date DESC, r.StartTime DESC";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reservations.Add(MapReservationFromReader(reader));
                        }
                    }
                }
            }

            return reservations;
        }

        public async Task<List<Reservation>> GetReservationsByRoomIdAsync(int roomId)
        {
            var reservations = new List<Reservation>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT r.*, u.Username, u.FirstName, u.LastName, rm.Name as RoomName
                    FROM Reservations r
                    JOIN Users u ON r.UserId = u.Id
                    JOIN Rooms rm ON r.RoomId = rm.Id
                    WHERE r.RoomId = @RoomId
                    ORDER BY r.Date ASC, r.StartTime ASC";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", roomId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reservations.Add(MapReservationFromReader(reader));
                        }
                    }
                }
            }

            return reservations;
        }


        public async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT r.*, u.Username, u.FirstName, u.LastName, rm.Name as RoomName
                    FROM Reservations r
                    JOIN Users u ON r.UserId = u.Id
                    JOIN Rooms rm ON r.RoomId = rm.Id
                    WHERE r.Id = @ReservationId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationId", reservationId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapReservationFromReader(reader);
                        }
                    }
                }
            }

            return null;
        }

        public async Task<int> AddReservationAsync(Reservation reservation)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Najpierw sprawdź dostępność
                var roomService = new RoomService(_connectionString);
                bool isAvailable = await roomService.CheckRoomAvailabilityAsync(
                    reservation.RoomId, reservation.Date, reservation.StartTime);

                if (!isAvailable)
                {
                    throw new InvalidOperationException("Wybrany termin jest już zajęty.");
                }

                string query = @"
                    INSERT INTO Reservations (UserId, RoomId, Date, StartTime, 
                                             ParticipantsCount, Status, CreatedAt, IsPaid)
                    VALUES (@UserId, @RoomId, @Date, @StartTime, 
                           @ParticipantsCount, @Status, @CreatedAt, @IsPaid);
                    SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", reservation.UserId);
                    command.Parameters.AddWithValue("@RoomId", reservation.RoomId);
                    command.Parameters.AddWithValue("@Date", reservation.Date.Date);
                    command.Parameters.AddWithValue("@StartTime", reservation.StartTime);
                    command.Parameters.AddWithValue("@ParticipantsCount", reservation.ParticipantsCount);
                    command.Parameters.AddWithValue("@Status", reservation.Status.ToString());
                    command.Parameters.AddWithValue("@CreatedAt", reservation.CreatedAt);
                    command.Parameters.AddWithValue("@IsPaid", reservation.IsPaid);

                    // Zwraca ID nowo utworzonej rezerwacji
                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        public async Task<bool> UpdateReservationAsync(Reservation reservation)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    UPDATE Reservations
                    SET UserId = @UserId,
                        RoomId = @RoomId,
                        Date = @Date,
                        StartTime = @StartTime,
                        ParticipantsCount = @ParticipantsCount,
                        Status = @Status,
                        IsPaid = @IsPaid
                    WHERE Id = @Id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", reservation.Id);
                    command.Parameters.AddWithValue("@UserId", reservation.UserId);
                    command.Parameters.AddWithValue("@RoomId", reservation.RoomId);
                    command.Parameters.AddWithValue("@Date", reservation.Date.Date);
                    command.Parameters.AddWithValue("@StartTime", reservation.StartTime);
                    command.Parameters.AddWithValue("@ParticipantsCount", reservation.ParticipantsCount);
                    command.Parameters.AddWithValue("@Status", reservation.Status.ToString());
                    command.Parameters.AddWithValue("@IsPaid", reservation.IsPaid);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }


        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Reservations SET Status = 'Cancelled' WHERE Id = @ReservationId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationId", reservationId);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }


        public async Task<List<TimeSpan>> GetAvailableTimeSlotsAsync(int roomId, DateTime date)
        {
            // Standardowe godziny otwarcia (np. 10:00 - 22:00)
            var openingTime = new TimeSpan(10, 0, 0);
            var closingTime = new TimeSpan(22, 0, 0);

            // Pobierz czas trwania sesji dla danego pokoju
            int sessionDuration = 0;
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string durationQuery = "SELECT DurationMinutes FROM Rooms WHERE Id = @RoomId";
                using (var command = new MySqlCommand(durationQuery, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", roomId);
                    var result = await command.ExecuteScalarAsync();
                    sessionDuration = Convert.ToInt32(result);
                }
            }

            // Dodaj czas na przygotowanie pokoju między sesjami (np. 30 minut)
            int sessionWithBreak = sessionDuration + 30;

            // Generuj możliwe terminy
            var potentialSlots = new List<TimeSpan>();
            for (TimeSpan time = openingTime; time.Add(TimeSpan.FromMinutes(sessionDuration)) <= closingTime; time = time.Add(TimeSpan.FromMinutes(sessionWithBreak)))
            {
                potentialSlots.Add(time);
            }

            // Pobierz zajęte terminy
            var bookedSlots = new List<TimeSpan>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"
                    SELECT StartTime 
                    FROM Reservations 
                    WHERE RoomId = @RoomId 
                    AND Date = @Date 
                    AND Status IN ('Pending', 'Confirmed')";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", roomId);
                    command.Parameters.AddWithValue("@Date", date.Date);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            bookedSlots.Add(reader.GetTimeSpan("StartTime"));
                        }
                    }
                }
            }

            // Usuń zajęte terminy z potencjalnych terminów
            return potentialSlots.Where(slot => !bookedSlots.Contains(slot)).ToList();
        }

        private Reservation MapReservationFromReader(MySqlDataReader reader)
        {
            var statusString = reader.GetString("Status");
            ReservationStatus status;
            Enum.TryParse(statusString, out status);

            return new Reservation
            {
                Id = reader.GetInt32("Id"),
                UserId = reader.GetInt32("UserId"),
                RoomId = reader.GetInt32("RoomId"),
                Date = reader.GetDateTime("Date"),
                StartTime = reader.GetTimeSpan("StartTime"),
                ParticipantsCount = reader.GetInt32("ParticipantsCount"),
                Status = status,
                CreatedAt = reader.GetDateTime("CreatedAt"),
                IsPaid = reader.GetBoolean("IsPaid"),
                User = new User
                {
                    Id = reader.GetInt32("UserId"),
                    Username = reader.GetString("Username"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName")
                },
                Room = new Room
                {
                    Id = reader.GetInt32("RoomId"),
                    Name = reader.GetString("RoomName")
                }
            };
        }
    }
}
