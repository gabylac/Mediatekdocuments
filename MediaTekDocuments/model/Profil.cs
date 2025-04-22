using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// classe interne qui mémorise les informations d'authentification
    /// </summary>
    public class Profil
    {
        public string Login { get; }
        public string Pwd { get; }
        public Profil(string login, string pwd)
        {
            Login = login;
            Pwd = pwd;
        }
    }
}
