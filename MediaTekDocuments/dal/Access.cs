using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using System.Diagnostics.Eventing.Reader;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        private const String PUT = "PUT";
        /// <summary>
        /// méthode http pour delete
        /// </summary>
        private const String DELETE = "DELETE";
        
        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString = null;
            try
            {
                authenticationString = ConfigurationManager.AppSettings["login"];
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if(instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }

        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message, String parametres)
        {
            // trans
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api                        
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());                        
                    }                    
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : "+e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

        /// <summary>
        /// retourne tous les suivis à partir de la BDD
        /// </summary>
        /// <returns>liste d'objets suivi</returns>
        public List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = TraitementRecup<Suivi>(GET, "suivi", null);
            return lesSuivis;
        }

        /// <summary>
        /// retourne toutes les commandes à partir de la BDD
        /// </summary>
        /// <returns>liste d'objets commande</returns>
        public List<Commande> GetAllCommandes()
        {
            List<Commande> lesCommandes = TraitementRecup<Commande>(GET, "commande", null);
            return lesCommandes;
        }

        /// <summary>
        /// retourne la liste des commandes d'un document
        /// </summary>
        /// <param name="idLivreDvd">id du document concerné</param>
        /// <returns>liste d'objets commandedocument</returns>
        public List<CommandeDocument> GetCommandesDocument(string idLivreDvd)
        {
            String jsonIdDocument = convertToJson("idLivreDvd", idLivreDvd);
            List<CommandeDocument> lesCommandes = TraitementRecup<CommandeDocument>(GET, "commandedocument/" + jsonIdDocument, null);
            return lesCommandes;
        }

        /// <summary>
        /// ecriture d'une commande en base de données
        /// </summary>
        /// <param name="commande">commande à insérer</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerCommande(Commande commande)
        {
            String jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            try
            {
                List<Commande> liste = TraitementRecup<Commande>(POST, "commande", "champs=" + jsonCommande);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// ecriture d'une commande d'un document en base de données
        /// </summary>
        /// <param name="commandeDocument">commandedocument à insérer</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument)
        {
            String jsonCommandeDoc = JsonConvert.SerializeObject(commandeDocument, new CustomDateTimeConverter());
            
           try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(POST, "commandedocument", "champs=" + jsonCommandeDoc);                
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// met à jour le statut d'une commande
        /// </summary>
        /// <param name="commande">commande avec les paramètres modifiés</param>
        /// <param name="idCommande">id de la commande concernée</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool UpdateCommandeDocument(CommandeDocument commande, string idCommande)
        {
            String jsonIdCommande = idCommande;
            String jsonCommande = JsonConvert.SerializeObject(commande);
            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(PUT, "commandedocument/" + jsonIdCommande, "champs=" + jsonCommande);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// supprime une commande d'un document
        /// </summary>
        /// <param name="idCommande">id de la commande concernée</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool DeleteCommande(string idCommande)
        {
            String jsonIdCommande = convertToJson("id", idCommande);
            try
            {
                List<Commande> lesCommandes = TraitementRecup<Commande>(DELETE, "commande/" + jsonIdCommande, null);
                return (lesCommandes != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// retourne la liste des abonnements d'une revue
        /// </summary>
        /// <param name="idRevue">id de la revue concernée</param>
        /// <returns>liste d'objets abonnement</returns>
        public List<Abonnement> GetAbonnementsRevue(string idRevue)
        {
            String jsonIdRevue = convertToJson("idRevue", idRevue);
            List<Abonnement> lesAbonnements = TraitementRecup<Abonnement>(GET, "abonnement/" + jsonIdRevue, null);
            return lesAbonnements;
        }

        /// <summary>
        /// écriture d'un abonnement en base de données
        /// </summary>
        /// <param name="abonnement">abonnement à insérer</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerAbonnementRevue(Abonnement abonnement)
        {            
            String jsonAbonnement = JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter());

            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(POST, "abonnement", "champs=" + jsonAbonnement);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// retourne tous les abonnements à partir de la BDD
        /// </summary>
        /// <returns>liste d'objets abonnement</returns>
        public List<Abonnement> GetAbonnements()
        {
            List<Abonnement> lesAbonnements = TraitementRecup<Abonnement>(GET, "abonnement", null);
            return lesAbonnements;
        }

        /// <summary>
        /// convertit en json un profil contenant les couples objet/valeur
        /// login et pwd
        /// </summary>
        /// <param name="profil"></param>
        /// <returns>les couples au format json</returns>
        private string ConvertProfilToJson(Profil profil)
        {
            var dict = new Dictionary<string, string>
        {
            { "login", profil.Login },
            { "pwd", profil.Pwd }
        };
            return JsonConvert.SerializeObject(dict);
        }

        /// <summary>
        /// controle qu'un utilisateur est autorisé à se connecter
        /// </summary>
        /// <param name="profil">objet profil de l'utilisateur concerné</param>
        /// <returns>liste d'objet users si l'utilisateur est présent dans la BDD</returns>
        public List<Users> ControleAuthentification(Profil profil)
        {
            String jsonProfil = ConvertProfilToJson(profil);
            try
            {
                List<Users> lesUsers = TraitementRecup<Users>(GET, "users/" + jsonProfil, null);
                if(lesUsers != null && lesUsers.Count > 0)
                {
                    return lesUsers;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// retourne la liste des users à partir de la BDD
        /// </summary>
        /// <returns>liste d'objets users</returns>
        public List<Users> GetAllUsers()
        {
            List<Users> lesUsers = TraitementRecup<Users>(GET, "users", null);
            return lesUsers;
        }
    }
}
