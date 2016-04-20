using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL.Mapper
{
    public class ReservatieMapper : EntityTypeConfiguration<Reservatie>
    {

        public ReservatieMapper()
        {
            ToTable("reservaties");

            //Key
            HasKey(r => r.Id);

            //Properties
            Property(r => r.Id)
                .HasColumnName("ID")
                .HasColumnType("numeric");

            Property(r => r.Ophaalmoment)
                .HasColumnName("OPHAALMOMENT")
                .HasColumnType("datetime");

            Property(r => r.Indienmoment)
                .HasColumnName("INDIENMOMENT")
                .HasColumnType("datetime");

            Property(r => r.Reservatiemoment)
                .HasColumnName("RESERVATIEMOMENT")
                .HasColumnType("datetime");

            Property(r => r.Opgehaald)
                .HasColumnName("OPGEHAALD")
                .HasColumnType("bit");

            //Relationships
            HasRequired(r => r.Lener).WithMany().Map(r =>
            {
                r.MapKey("LENER_ID");
            });

            //ReservatieLijnen bestaat nog niet

            //HasMany(r => r.ReservatieLijnen)
            //    .WithMany()
            //    .Map(r =>
            //    {
            //        r.MapLeftKey("RESERVATIE_ID");
            //        r.MapRightKey("RESERVATIELIJN_ID");
            //        r.ToTable("reservatie_reservatielijnen");
            //    });


        }

    }
}