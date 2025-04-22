using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Service
    {
        public string Id { get; set; }
        public string Libelle { get; set; }

        public Service (string id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }
    }
}
