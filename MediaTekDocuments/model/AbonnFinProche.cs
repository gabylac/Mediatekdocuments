using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// classe métier interne pour mémoriser les informations
    /// des abonnements dont la fin est proche
    /// </summary>
    public class AbonnFinProche
    {
        public string TitreRevue { get; set; }
        public DateTime DateFin { get; set; }

        /// <summary>
        /// valorise les propriétés
        /// </summary>
        /// <param name="titreRevue"></param>
        /// <param name="dateFin"></param>
        public AbonnFinProche(string titreRevue, DateTime dateFin)
        {
            TitreRevue = titreRevue;
            DateFin = dateFin;
        }
    }
}
