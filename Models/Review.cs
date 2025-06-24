using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EscapeRoom.Models
{
    public class Review
    {
        public int RecenzjaId { get; set; }
        public int UzytkownikId { get; set; }
        public int PokojId { get; set; }
        public byte Ocena { get; set; }
        public string Komentarz { get; set; }
        public DateTime DataUtworzenia { get; set; }

        // Optional: Navigation properties
        public virtual User User { get; set; }
        public virtual Room Room { get; set; }

        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
    }
}