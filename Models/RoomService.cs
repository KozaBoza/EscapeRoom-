using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient; //do zmiany
using EscapeRoom.Models;
using System.Data.SqlClient;

//TUTAJ TRZEBA KOMPLETNIE WSZYSTKO POPOPRAWIAC -- ZALEZY OD BAZ DANYCH
namespace EscapeRoom.Services
{
    public class RoomService
    {
        private readonly string _connectionString;

        public RoomService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Room>> GetAllRoomsAsync(bool includeInactive = false)
        {
            var rooms = new List<Room>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Rooms";
                if (!includeInactive)
                {
                    query += " WHERE IsActive = 1";
                }

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        rooms.Add(new Room //do zmiany
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Description = reader.GetString("Description"),
                            Difficulty = reader.GetInt32("Difficulty"),
                            MaxParticipants = reader.GetInt32("MaxParticipants"),
                            DurationMinutes = reader.GetInt32("DurationMinutes"),
                            Price = reader.GetDecimal("Price"),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString("ImageUrl"),
                            IsActive = reader.GetBoolean("IsActive"),
                            AverageRating = reader.IsDBNull(reader.GetOrdinal("AverageRating")) ? 0 : reader.GetDouble("AverageRating")
                        });
                    }
                }
            }

            //pobiera recenzje dla pokoju
            foreach (var room in rooms)
            {
                room.Reviews = await GetReviewsForRoomAsync(room.Id);
            }

            return rooms;
        }


        public async Task<Room> GetRoomByIdAsync(int roomId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Rooms WHERE Id = @RoomId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", roomId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var room = new Room
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Description = reader.GetString("Description"),
                                Difficulty = reader.GetInt32("Difficulty"),
                                MaxParticipants = reader.GetInt32("MaxParticipants"),
                                DurationMinutes = reader.GetInt32("DurationMinutes"),
                                Price = reader.GetDecimal("Price"),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString("ImageUrl"),
                                IsActive = reader.GetBoolean("IsActive"),
                                AverageRating = reader.IsDBNull(reader.GetOrdinal("AverageRating")) ? 0 : reader.GetDouble("AverageRating")
                            };

                            room.Reviews = await GetReviewsForRoomAsync(room.Id);

                            return room;
                        }
                    }
                }
            }

            return null;
        }


        public async Task<int> AddRoomAsync(Room room)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    INSERT INTO Rooms (Name, Description, Difficulty, MaxParticipants, 
                                      DurationMinutes, Price, ImageUrl, IsActive)
                    VALUES (@Name, @Description, @Difficulty, @MaxParticipants, 
                           @DurationMinutes, @Price, @ImageUrl, @IsActive);
                    SELECT LAST_INSERT_ID();"
                ;

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", room.Name);
                    command.Parameters.AddWithValue("@Description", room.Description);
                    command.Parameters.AddWithValue("@Difficulty", room.Difficulty);
                    command.Parameters.AddWithValue("@MaxParticipants", room.MaxParticipants);
                    command.Parameters.AddWithValue("@DurationMinutes", room.DurationMinutes);
                    command.Parameters.AddWithValue("@Price", room.Price);
                    command.Parameters.AddWithValue("@ImageUrl", (object)room.ImageUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", room.IsActive);

                    // Zwraca ID nowo utworzonego pokoju
                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        public async Task<bool> UpdateRoomAsync(Room room)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    UPDATE Rooms
                    SET Name = @Name,
                        Description = @Description,
                        Difficulty = @Difficulty,
                        MaxParticipants = @MaxParticipants,
                        DurationMinutes = @DurationMinutes,
                        Price = @Price,
                        ImageUrl = @ImageUrl,
                        IsActive = @IsActive
                    WHERE Id = @Id;"
                ;

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", room.Id);
                    command.Parameters.AddWithValue("@Name", room.Name);
                    command.Parameters.AddWithValue("@Description", room.Description);
                    command.Parameters.AddWithValue("@Difficulty", room.Difficulty);
                    command.Parameters.AddWithValue("@MaxParticipants", room.MaxParticipants);
                    command.Parameters.AddWithValue("@DurationMinutes", room.DurationMinutes);
                    command.Parameters.AddWithValue("@Price", room.Price);
                    command.Parameters.AddWithValue("@ImageUrl", (object)room.ImageUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", room.IsActive);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }


        public async Task<bool> DeactivateRoomAsync(int roomId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Rooms SET IsActive = 0 WHERE Id = @RoomId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", roomId);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }

        private async Task<System.Collections.ObjectModel.ObservableCollection<Review>> GetReviewsForRoomAsync(int roomId)
        {
            var reviews = new System.Collections.ObjectModel.ObservableCollection<Review>();

            using (var connection = new MySqlConnection(_connectionString))
            {        }
                await connection.OpenAsync();

                string query = @"
                    SELECT r.*, u.Username, u.FirstName, u.LastName
                    FROM Reviews r
                    JOIN Users u ON r.UserId = u.Id
                    WHERE r.RoomId = @RoomId
                    ORDER BY r.CreatedAt DESC"
                ;

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", roomId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var review = new Review
                            {
                                Id = reader.GetInt32("Id"),
                                UserId = reader.GetInt32("UserId"),
                                RoomId = reader.GetInt32("RoomId"),
                                Rating = reader.GetInt32("Rating"),
                                Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? null : reader.GetString("Comment"),
                                CreatedAt = reader.GetDateTime("CreatedAt"),
                                User = new User
                                {
                                    Id = reader.GetInt32("UserId"),
                                    Username = reader.GetString("Username"),
                                    FirstName = reader.GetString("FirstName"),
                                    LastName = reader.GetString("LastName")
                                }
                            };
                            reviews.Add(review);
                        }
                    }
                }
            }

            return reviews;
        }

        public async Task<bool> CheckRoomAvailabilityAsync(int roomId, DateTime date, TimeSpan startTime)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                //sprawdzenie
                string query = @"
                    SELECT COUNT(*) 
                    FROM Reservations 
                    WHERE RoomId = @RoomId 
                    AND Date = @Date 
                    AND StartTime = @StartTime
                    AND Status IN ('Pending', 'Confirmed')";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", roomId);
                    command.Parameters.AddWithValue("@Date", date.Date);
                    command.Parameters.AddWithValue("@StartTime", startTime);

                    int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count == 0; 
                }
            }
        }
    }