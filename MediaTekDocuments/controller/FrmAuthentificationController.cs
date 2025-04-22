using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.controller
{
    class FrmAuthentificationController
    {
        private readonly Access access;
        public FrmAuthentificationController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// verifie l'authentification
        /// </summary>
        /// <param name="profil">objet contenant les infos du profil à vérifier</param>
        /// <returns>true si les infos sont correctes</returns>
        public List<Users> ControleAuthentification(Profil profil)
        {
            return access.ControleAuthentification(profil);
        }

        /// <summary>
        /// retourne la liste des users
        /// </summary>
        /// <returns>liste d'objets users</returns>
        public List<Users> GetAllUsers()
        {
            return access.GetAllUsers();
        }
    }
}
