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
using Microsoft.EntityFrameworkCore;


namespace EscapeRoom.Helpers
{
    public class EscapeRoomContext : DbContext
    {
        public DbSet<User> Uzytkownicy { get; set; }
        public DbSet<Room> Pokoje { get; set; }
        public DbSet<Reservation> Rezerwacje { get; set; }


        public EscapeRoomContext(DbContextOptions<EscapeRoomContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    "Server=localhost;;", //do ustawienia
                    new MySqlServerVersion(new Version(8, 0, 21))
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasCharSet("latin1", DelegationModes.ApplyToAll);
            modelBuilder.Entity<Reservation>()
                .Property(e => e.Status)
                .HasConversion<string>()
                .HasColumnType("enum('zarezerwowana','odwolana','zrealizowana')");

            //z email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.DataRejestracji)
                .HasDefaultValueSql("current_timestamp()");

            modelBuilder.Entity<User>()
                .Property(u => u.Admin)
                .HasDefaultValue(false);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.DataUtworzenia)
                .HasDefaultValueSql("current_timestamp()");

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Uzytkownik)
                .WithMany(u => u.Rezerwacje)
                .HasForeignKey(r => r.UzytkownikId)
                .HasConstraintName("Rezerwacje_ibfk_1")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Pokoj)
                .WithMany(p => p.Rezerwacje)
                .HasForeignKey(r => r.PokojId)
                .HasConstraintName("Rezerwacje_ibfk_2")
                .OnDelete(DeleteBehavior.Restrict);

            // Konfiguracja typów kolumn
            modelBuilder.Entity<Room>()
                .Property(r => r.Cena)
                .HasColumnType("decimal(6,2)");

            modelBuilder.Entity<User>().ToTable("Uzytkownicy", t => t.HasComment("InnoDB"));
            modelBuilder.Entity<Room>().ToTable("Pokoje", t => t.HasComment("InnoDB"));
            modelBuilder.Entity<Reservation>().ToTable("Rezerwacje", t => t.HasComment("InnoDB"));

        }

        public static class DbInitializer
        {
            public static void Initialize(EscapeRoomContext context)
            {
                context.Database.EnsureCreated();

                //czy baza zawiera już dane
                if (context.Pokoje.Any())
                {
                    return; 
                }

                var pokoje = new Room[]
                {
 
                };

                context.Pokoje.AddRange(pokoje);
                context.SaveChanges();
            }
        }
    }

}