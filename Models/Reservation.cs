using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
//dane dot. rezerwacji
namespace EscapeRoom.Models
{
    public enum ReservationStatus
    {
        zarezerwowana,
        odwolana,
        zrealizowana
    }

    [Table("Rezerwacje")]
    public class Reservation
    {
        [Key]
        [Column("rezerwacja_id")]
        public int RezerwacjaId { get; set; }

        [Required]
        [Column("uzytkownik_id")]
        public int UzytkownikId { get; set; }

        [Required]
        [Column("pokoj_id")]
        public int PokojId { get; set; }

        [Required]
        [Column("data_rozpoczecia")]
        public DateTime DataRozpoczecia { get; set; }

        [Required]
        [Column("liczba_osob")]
        public byte LiczbaOsob { get; set; }

        [Required]
        [Column("status")]
        public ReservationStatus Status { get; set; } = ReservationStatus.zarezerwowana;

        [Column("data_utworzenia")]
        public DateTime DataUtworzenia { get; set; } = DateTime.Now;

        [ForeignKey("UzytkownikId")]
        public virtual User Uzytkownik { get; set; }

        [ForeignKey("PokojId")]
        public virtual Room Pokoj { get; set; }

    }
}