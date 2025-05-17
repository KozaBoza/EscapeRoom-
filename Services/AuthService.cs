using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using EscapeRoom.Models;
using System.Data.SqlClient;

namespace EscapeRoom.Services
{
    public class AuthService
    {
        private readonly string _connectionString;
        private User _currentUser;

        public AuthService(string connectionString)
        {
            _connectionString = connectionString;
        }


        public User CurrentUser
        {
            get { return _currentUser; }
            private set { _currentUser = value; }
        }


        public bool IsUserLoggedIn => CurrentUser != null;
        public bool IsUserAdmin => IsUserLoggedIn && CurrentUser.IsAdmin;
        public async Task<bool> RegisterUserAsync(string username, string password, string email,
            string firstName, string lastName, string phoneNumber)
        {
            // sprawdzenie  czy użytkownik o takiej nazwie już istnieje
            if (await UserExistsAsync(username))
            {
                throw new InvalidOperationException("Użytkownik o takiej nazwie już istnieje.");
            }

            if (await EmailExistsAsync(email))
            {
                throw new InvalidOperationException("Podany adres email jest już używany.");
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string passwordHash = HashPassword(password);

                string query = @"
                    INSERT INTO Users (Username, PasswordHash, Email, FirstName, LastName, 
                                      PhoneNumber, IsAdmin, RegistrationDate)
                    VALUES (@Username, @PasswordHash, @Email, @FirstName, @LastName, 
                           @PhoneNumber, 0, @RegistrationDate)"
                ;

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        //logowanie 
        public async Task<bool> LoginAsync(string username, string password)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string passwordHash = HashPassword(password);

                string query = @"
                    SELECT * FROM Users 
                    WHERE Username = @Username AND PasswordHash = @PasswordHash"
                ;

               // using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            CurrentUser = new User
                            {
                                Id = reader.GetInt32("Id"),
                                Username = reader.GetString("Username"),
                                Email = reader.GetString("Email"),
                                FirstName = reader.GetString("FirstName"),
                                LastName = reader.GetString("LastName"),
                                PhoneNumber = reader.GetString("PhoneNumber"),
                                IsAdmin = reader.GetBoolean("IsAdmin"),
                                RegistrationDate = reader.GetDateTime("RegistrationDate")
                            };
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //wylogowanie użytkownika
        public void Logout()
        {
            CurrentUser = null;
        }
        private async Task<bool> UserExistsAsync(string username)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        private async Task<bool> EmailExistsAsync(string email)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}