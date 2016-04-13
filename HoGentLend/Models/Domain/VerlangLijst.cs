using System;
using System.Collections.Generic;
using System.Linq;

namespace HoGentLend.Models.Domain
{
    public class VerlangLijst
    {
        public int VerlangLijstId { get; set; }

        public IList<Materiaal> Materials { get; set; }

        public void addMaterial(Materiaal material)
        {
            if (material != null && Materials.FirstOrDefault(m => m.Name == material.Name) != null)
                throw new ArgumentException("Deze verlanglijst heeft al een materiaal met dezelfde naam");
            Materials.Add(material);
        }

        public void removeMaterial(Materiaal material)
        {
            if (!Materials.Contains(material))
                throw new ArgumentException(string.Format("De verlanglijst bevat het materiaal {0} niet.", material.Name));
            Materials.Remove(material);
        }

        public Materiaal FindBy(int materiaalId)
        {
            return Materials.FirstOrDefault(m => m.Id == materiaalId);
        }

    }
}