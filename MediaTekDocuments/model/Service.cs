namespace MediaTekDocuments.model
{
    /// <summary>
    /// classe métier liée à la table service
    /// </summary>
    public class Service
    {
        public string Id { get; set; }
        public string Libelle { get; set; }

        /// <summary>
        /// valorise les propriétés
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Service(string id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }
    }
}
