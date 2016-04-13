using HoGentLend.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.DAL.Mapper
{
    public class GebruikerMapper : EntityTypeConfiguration<Gebruiker>
    {
        public GebruikerMapper()
        {
            ToTable("gebruikers");

            // Key
            HasKey(g => g.Id);

            // Properties
            Property(g => g.Id)
                .HasColumnName("ID")
                .HasColumnType("numeric");

            Property(g => g.LastName)
                .HasColumnName("ACHTERNAAM")
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(255);

            Property(g => g.FirstName)
                .HasColumnName("VOORNAAM")
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(255);

            Property(g => g.Email)
                .HasColumnName("EMAIL")
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(255);

            Property(g => g.IsLector)
                .HasColumnName("LECTOR")
                .HasColumnType("bit")
                .IsOptional();

            // Relationships
            HasOptional(t => t.WishList)
                .WithMany()
                .Map(m => m.MapKey("VerlangLijstId"));
        }
    }
}