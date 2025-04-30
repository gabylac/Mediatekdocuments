namespace MediaTekDocuments.model
{
    /// <summary>
    /// classe interne qui mémorise les informations d'authentification
    /// </summary>
    public class Profil
    {
        public string Login { get; }
        public string Pwd { get; }

        /// <summary>
        /// valorise les propriétés
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pwd"></param>
        public Profil(string login, string pwd)
        {
            Login = login;
            Pwd = pwd;
        }
    }
}
