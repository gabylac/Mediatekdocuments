namespace MediaTekDocuments.model
{
    /// <summary>
    /// classe métier liée à la table users
    /// </summary>
    public class Users
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Pwd { get; set; }
        public string IdService { get; set; }
        
        /// <summary>
        /// valorise les prorpiétés
        /// </summary>
        /// <param name="id"></param>
        /// <param name="login"></param>
        /// <param name="idService"></param>
        public Users(string id, string login, string idService)
        {
            this.Id = id;
            this.Login = login;
            this.Pwd = Pwd;
            this.IdService = idService;            
        }
    }
}
