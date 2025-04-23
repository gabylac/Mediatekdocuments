namespace MediaTekDocuments.model
{
    public class Users
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Pwd { get; set; }
        public string IdService { get; set; }
        public Service Service { get; set; }

        public Users(string id, string login, string pwd, string idService, Service service)
        {
            this.Id = id;
            this.Login = login;
            this.Pwd = pwd;
            this.IdService = idService;
            this.Service = service;
        }
    }
}
