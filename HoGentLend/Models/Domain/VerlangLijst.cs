using System;
using System.Collections.Generic;
using System.Linq;

namespace HoGentLend.Models.Domain
{
    public class VerlangLijst
    { 

        public long Id { get; set; }

        public virtual IList<Materiaal> Materials { get; set; }

        public VerlangLijst()
        {
            Materials = new List<Materiaal>();
        }

        public void AddMaterial(Materiaal material)
        {
            if (material != null && Materials.FirstOrDefault(m => m.Name == material.Name) != null)
                throw new ArgumentException("Deze verlanglijst heeft al een materiaal met dezelfde naam");
            Materials.Add(material);
        }

        public void RemoveMaterial(Materiaal material)
        {
            if (!Materials.Contains(material))
                throw new ArgumentException(string.Format("De verlanglijst bevat het materiaal {0} niet.", material.Name));
            Materials.Remove(material);
        }

        public Materiaal FindBy(int materiaalId)
        {
            return Materials.FirstOrDefault(m => m.Id == materiaalId);
        }

        public bool Exists(long id)
        {
            return Materials.Any(m => m.Id == id);
        }
    }
}