using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscapeRoom.Models
{
    [Table("Pokoje")]
    public class Room
    {
        [Key]
        [Column("pokoj_id")]
        public int PokojId { get; set; }

        [Required]
        [StringLength(150)]
        [Column("nazwa")]
        public string Nazwa { get; set; }

        [Column("opis", TypeName = "text")]
        public string Opis { get; set; }

        [Required]
        [Column("trudnosc")]
        public byte Trudnosc { get; set; }

        [Required]
        [Column("cena", TypeName = "decimal(6,2)")]
        public decimal Cena { get; set; } //zmienic cenazagodzine na cena

        [Required]
        [Column("max_graczy")]
        public byte MaxGraczy { get; set; }

        [Required]
        [Column("czas_minut")]
        public int CzasMinut { get; set; }

        public virtual ICollection<Reservation> Rezerwacje { get; set; } = new List<Reservation>();
    }
}