﻿using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System.Collections.Generic;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// contrôleur de FrmAlerteAbonnementController
    /// </summary>
    class FrmAlerteAbonnementController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmAlerteAbonnementController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// récupère la liste des abonnements
        /// </summary>
        /// <returns>liste d'objets abonnement</returns>
        public List<Abonnement> GetAbonnements()
        {
            return access.GetAbonnements();
        }

        /// <summary>
        /// récupère la liste des revues
        /// </summary>
        /// <returns>liste d'objets revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }
    }
}
