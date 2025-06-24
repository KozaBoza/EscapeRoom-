using EscapeRoom.Models;
using System;

public class Review
{
    public int RecenzjaId { get; set; }
    public int UzytkownikId { get; set; }
    public int PokojId { get; set; }
    public byte Ocena { get; set; }
    public string Komentarz { get; set; }
    public DateTime DataUtworzenia { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual Room Room { get; set; }
}