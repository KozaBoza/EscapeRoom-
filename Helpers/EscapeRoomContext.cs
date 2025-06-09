using EscapeRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace EscapeRoom.Helpers
{ //nie wiem czy jest to koniecznie, natomiast wstepnie przy probie integracji z bd
    public class EscapeRoomContext : DbContext
    {
        public EscapeRoomContext() : base("name=EscapeRoomDb")
        {
        }

        public DbSet<User> Uzytkownicy { get; set; }
        public DbSet<Room> Pokoje { get; set; }
        public DbSet<Reservation> Rezerwacje { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // tabela i kolumna
            modelBuilder.Entity<User>().ToTable("Uzytkownicy");
            modelBuilder.Entity<Room>().ToTable("Pokoje");
            modelBuilder.Entity<Reservation>().ToTable("Rezerwacje");

            modelBuilder.Entity<User>()
                .Property(u => u.DataRejestracji)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<User>()
                .Property(u => u.Admin)
                .HasColumnType("bit");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Room>()
                .Property(r => r.Cena)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.Status)
                .HasColumnType("nvarchar");

            modelBuilder.Entity<Reservation>()
                .HasRequired(r => r.Uzytkownik)
                .WithMany(u => u.Rezerwacje)
                .HasForeignKey(r => r.UzytkownikId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reservation>()
                .HasRequired(r => r.Pokoj)
                .WithMany(p => p.Rezerwacje)
                .HasForeignKey(r => r.PokojId)
                .WillCascadeOnDelete(false);
        }
    }

}