using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EscapeRoom.Models
{
    public class Contact
    {
        public int Id { get; set; } // Teraz z setterem, dla Id z bazy danych
        public string Name { get; set; } // Zmieniono na Name, aby pasowało do ContactView
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime SubmittedAt { get; set; } // Data wysłania wiadomości
        public bool IsRead { get; set; } // Czy wiadomość została przeczytana przez administratora

        // Konstruktor domyślny (bez parametrów) jest potrzebny do deserializacji danych z bazy/JSON-a
        public Contact() { }

        // Opcjonalny konstruktor do szybkiego tworzenia instancji, np. przy wysyłaniu
        public Contact(string name, string email, string message)
        {
            // Id będzie ustawiane przez bazę danych po zapisie
            Name = name;
            Email = email;
            Message = message;
            SubmittedAt = DateTime.Now; // Ustaw domyślną datę utworzenia
            IsRead = false; // Domyślnie nowa wiadomość jest nieprzeczytana
        }
    }
}
