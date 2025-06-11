using System.Collections.Generic;
using EscapeRoom.Models;
using MySql.Data.MySqlClient;

namespace EscapeRoom.Services
{
    public class RoomService
    {
        private readonly DatabaseService _db = new DatabaseService();

        public List<Room> GetAllRooms()
        {
            var rooms = new List<Room>();
            string query = "SELECT * FROM pokoje";

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new Room
                        {
                            PokojId = reader.GetInt32("PokojId"),
                            Nazwa = reader.GetString("Nazwa"),
                            Opis = reader.GetString("Opis"),
                            Trudnosc = reader.GetByte("Trudnosc"),
                            Cena = reader.GetDecimal("Cena"),
                            MaxGraczy = reader.GetByte("MaxGraczy"),
                            CzasMinut = reader.GetInt32("CzasMinut")
                        });
                    }
                }
            }

            return rooms;
        }
    }
}
