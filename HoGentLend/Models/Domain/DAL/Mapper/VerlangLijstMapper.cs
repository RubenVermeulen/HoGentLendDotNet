using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;
using System.Data.Entity.ModelConfiguration;

namespace HoGentLend.Models.Domain.DAL.Mapper
{
    public class VerlangLijstMapper : EntityTypeConfiguration<VerlangLijst>
    {

        public VerlangLijstMapper()
        {
            ToTable("verlanglijstjes");

            // Key
            HasKey(v => v.VerlangLijstId);

            // Properties
            Property(v => v.VerlangLijstId)
                .HasColumnName("ID")
                .HasColumnType("numeric");

            //Relationships
            HasMany(v => v.Materials)
                .WithRequired()
                .Map(m => m.MapKey("VerlanglijstId"))
                .WillCascadeOnDelete(false);

        }
    }
}