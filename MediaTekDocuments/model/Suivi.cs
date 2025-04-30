namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Suivi (statut d'une commande)
    /// </summary>
    public class Suivi
    {
        public string Id { get; set; }
        public string Libelle { get; set; }

        /// <summary>
        /// valorise les propriétés
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Suivi(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combos
        /// </summary>
        /// <returns>Libelle</returns>
        public override string ToString()
        {
            return this.Libelle;
        }
    }
}
