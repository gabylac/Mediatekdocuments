﻿using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    public class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// getter sur les suivis
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }

        /// <summary>
        /// getter sur les commandes
        /// </summary>
        /// <returns>Liste d'objets commande</returns>
        public List<Commande> GetAllCommandes()
        {
            return access.GetAllCommandes();
        }

        /// <summary>
        /// récupère la liste des commandes d'un document
        /// </summary>
        /// <param name="idLivreDvd"></param>
        /// <returns>liste d'objets commandeDocument</returns>
        public List<CommandeDocument> GetCommandesDocument(string idLivreDvd)
        {
            return access.GetCommandesDocument(idLivreDvd);
        }

        /// <summary>
        /// ajoute une commande à la liste des commandes
        /// </summary>
        /// <param name="commande">objet commande à ajouter</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerCommande(Commande commande)
        {
            return access.CreerCommande(commande);
        }

        /// <summary>
        /// ajoute une commande à un document
        /// </summary>
        /// <param name="commandeDocument">objet commandedocument concerné</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument)
        {
            return access.CreerCommandeDocument(commandeDocument);
        }

        /// <summary>
        /// met à jour le staut d'une commande d'un document
        /// </summary>
        /// <param name="commande">objet commande concerné avec les paramètres à mettre à jour</param>
        /// <param name="idCommande">id de la commande concernée</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool UpdateCommandeDocument(CommandeDocument commande, string idCommande)
        {
            return access.UpdateCommandeDocument(commande, idCommande);
        }

        /// <summary>
        /// supprime une commande d'un document
        /// </summary>
        /// <param name="idCommande">id de la commande concernée</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool DeleteCommande(string idCommande)
        {
            return access.DeleteCommande(idCommande);
        }

        /// <summary>
        /// récupère la liste des abonnements d'une revue
        /// </summary>
        /// <param name="idRevue">id de la revue concernée</param>
        /// <returns>liste d'objets abonnement</returns>
        public List<Abonnement> GetAbonnementsRevue(string idRevue)
        {
            return access.GetAbonnementsRevue(idRevue);
        }

        /// <summary>
        /// ajoute un abonnement à une revue
        /// </summary>
        /// <param name="abonnement">objet abonnement concerné</param>
        /// <returns>true si l'ajout a pu se faire</returns>
        public bool CreerAbonnementRevue(Abonnement abonnement)
        {
            return access.CreerAbonnementRevue(abonnement);
        }

        /// <summary>
        /// controle que la date de parution des exemplaires se trouve 
        /// entre la date de la commande et de la fin de l'abonnement
        /// avant de supprimer une commande d'une revue
        /// </summary>
        /// <param name="dateCommande">date de la commande concernée</param>
        /// <param name="dateFinAbonnement">date de la fin d'abonnement de la revue</param>
        /// <param name="dateParution">date de parution de l'exempplaire de la revue concerné</param>
        /// <returns>true si la date de parution est comprise entre les 2</returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            if (dateParution.CompareTo(dateCommande) >= 0 && dateParution.CompareTo(dateFinAbonnement) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
