using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// classe métier liée à la table commande
    /// </summary>
    public class Commande
    {
        public string Id { get; set; }
        public DateTime DateCommande { get; set; }
        public double Montant { get; set; }

        /// <summary>
        /// valorise les propriétés
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        public Commande(string id, DateTime dateCommande, double montant)
        {
            Id = id;
            DateCommande = dateCommande;
            Montant = montant;
        }

    }
}
