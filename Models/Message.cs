using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscapeRoom.Models
{
    [Table("Wiadomosci")]
    public class Message
    {
        [Key]
        [Column("wiadomosc_id")]
        public int WiadomoscId { get; set; }

        [Required]
        [Column("nadawca_id")]
        public int NadawcaId { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [StringLength(500)]
        [Column("tresc")]
        public string Tresc { get; set; }

        [ForeignKey("NadawcaId")]
        public virtual User Nadawca { get; set; }
    }
}

