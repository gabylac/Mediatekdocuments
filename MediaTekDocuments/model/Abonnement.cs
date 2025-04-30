using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// classe métier liée à la table abonnement
    /// </summary>
    public class Abonnement : Commande
    {
        public DateTime DateFinAbonnement { get; set; }
        public string IdRevue { get; set; }

        /// <summary>
        /// valorise les propriétés
        /// </summary>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="idRevue"></param>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        public Abonnement(DateTime dateFinAbonnement, string idRevue, string id, DateTime dateCommande, double montant) : base(id, dateCommande, montant)
        {
            this.DateFinAbonnement = dateFinAbonnement;
            this.IdRevue = idRevue;
        }
    }
}
