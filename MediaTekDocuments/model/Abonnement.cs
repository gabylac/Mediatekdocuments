using System;

namespace MediaTekDocuments.model
{
    public class Abonnement : Commande
    {
        public DateTime DateFinAbonnement { get; set; }
        public string IdRevue { get; set; }

        public Abonnement(DateTime dateFinAbonnement, string idRevue, string id, DateTime dateCommande, double montant) : base(id, dateCommande, montant)
        {
            this.DateFinAbonnement = dateFinAbonnement;
            this.IdRevue = idRevue;
        }
    }
}
