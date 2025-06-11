using System;
using EscapeRoom.Services;

namespace EscapeRoom.Services
{
    public class AuthService
    {
        private readonly DatabaseService _db;

        public AuthService()
        {
            _db = new DatabaseService();
        }

        public bool Login(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = PASSWORD(@password)";

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password); // ❗ hasło powinno być haszowane w produkcji

                    var result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result > 0;
                }
            }
        }
    }
}
