using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.EntityFrameworkCore;

namespace EscapeRoom.Models
{ //obsługa konta, trzeba przemyśleć jeszcze jak to zrobić
    [Table("Uzytkownicy")]
    public class User
    {
        [Key]
        [Column("uzytkownik_id")]
        public int UzytkownikId { get; set; }

        [Required]
        [StringLength(255)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        [Column("haslo_hash")]
        public string HasloHash { get; set; }

        [Required]
        [StringLength(100)]
        [Column("imie")]
        public string Imie { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nazwisko")]
        public string Nazwisko { get; set; }

        [StringLength(20)]
        [Column("telefon")]
        public string Telefon { get; set; }

        [Column("data_rejestracji")]
        public DateTime DataRejestracji { get; set; } = DateTime.Now;

        [Column("admin")]
        public bool Admin { get; set; } = false;

        public virtual ICollection<Reservation> Rezerwacje { get; set; } = new List<Reservation>();

    }
}