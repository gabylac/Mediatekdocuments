using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    public partial class FrmAlerteAbonnement : Form
    {
        private readonly FrmAlerteAbonnementController controller;
        private readonly BindingSource bdgAbonnementRevueListe = new BindingSource();

        /// <summary>
        /// création du constructeur lié à ce formulaire
        /// </summary>
        internal FrmAlerteAbonnement()
        {
            InitializeComponent();
            this.controller = new FrmAlerteAbonnementController();
            List<Abonnement> lesAbonnementsFinProche = AfficheListeFinAbonnProche(controller.GetAbonnements());
            RemplirDatagridListeFinAbonnement(lesAbonnementsFinProche);
        }

        /// <summary>
        /// retourne la liste des abonnements dont la fin est <30 jours
        /// </summary>
        /// <param name="lesAbonnements">liste de tous les abonnements</param>
        /// <returns>liste d'objets abonnement avec le critère voulu</returns>
        static private List<Abonnement> AfficheListeFinAbonnProche(List<Abonnement> lesAbonnements)
        {
            List<Abonnement> abonnementsFinProche = new List<Abonnement>();
            foreach (Abonnement unAbonnement in lesAbonnements)
            {
                if (unAbonnement.DateFinAbonnement.CompareTo(DateTime.Now.AddDays(30)) < 0)
                {
                    abonnementsFinProche.Add(unAbonnement);
                }
            }
            return abonnementsFinProche;
        }

        /// <summary>
        /// accès à la page principale de l'application : l'onglet livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            FrmMediatek frm = new FrmMediatek();
            frm.ShowDialog();

        }

        /// <summary>
        /// remplissage du datagrid avec les abonnements 
        /// dont la fin est < 30 jours
        /// </summary>
        /// <param name="abonnements">liste des abonnements concernés</param>
        private void RemplirDatagridListeFinAbonnement(List<Abonnement> abonnements)
        {
            List<Abonnement> finAbonnements = abonnements;
            List<Revue> revues = controller.GetAllRevues();

            // Création de la vue pour affichage
            List<AbonnFinProche> vues = finAbonnements.Select(a => new AbonnFinProche
            {
                TitreRevue = revues.FirstOrDefault(r => r.Id == a.IdRevue).Titre,
                DateFin = a.DateFinAbonnement,
            }).ToList();
            bdgAbonnementRevueListe.DataSource = vues;
            dgvListeFinAbonn.DataSource = bdgAbonnementRevueListe;
            dgvListeFinAbonn.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
    }
}
