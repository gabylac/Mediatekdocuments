using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// classe d'affichage fenetre authentification 
    /// </summary>
    public partial class FrmAuthentification : Form
    {
        private readonly FrmAuthentificationController controller;

        /// <summary>
        /// création du formulaire lié à ce controller
        /// </summary>
        public FrmAuthentification()
        {
            InitializeComponent();
            this.controller = new FrmAuthentificationController();
        }

        /// <summary>
        /// connection d'un utilisateur après vérification de son profil
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            String login = txtbLogin.Text;
            String pwd = txtbPwd.Text;
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(pwd))
            {
                Log.Information("Erreur de saisie des champs, les champs ne doivent pas être vide");
                MessageBox.Show("Tous les champs doivent être remplis", "Information");
                return;
            }
            Profil profil = new Profil(login, pwd);
            List<Users> lesUsers = controller.ControleAuthentification(profil);
            if (lesUsers != null && lesUsers.Count > 0)
            {
                Users userConnecte = lesUsers[0];
                string userIdService = userConnecte.IdService;
                if(userIdService != "1" && userIdService != "2" && userIdService != "3")
                {
                    Log.Information("Tentative de connexion d'un utilisateur non autorisé");
                    MessageBox.Show("Vous n'êtes pas autorisé à accéder à l'application");
                    Environment.Exit(0);
                    return;
                }
                if (userIdService == "1" || userIdService == "2")
                {
                    FrmAlerteAbonnement frm = new FrmAlerteAbonnement();
                    frm.ShowDialog();                    
                }
                else
                {
                    if (userIdService == "3")
                    {
                        FrmMediatek frm = new FrmMediatek();
                        frm.Show();
                        frm.groupBoxRecExemplaire.Enabled = false;
                        frm.tabComLivres.Enabled = false;
                        frm.tabComDVD.Enabled = false;
                        frm.tabAbonnement.Enabled = false;
                    }                    
                }
            }
            else
            {
                Log.Information("Tentative de connexion échouée");
                MessageBox.Show("Authentification incorrecte ou vous n'êtes pas autorisé à vous connecter", "Alerte");
                txtbLogin.Text = "";
                txtbPwd.Text = "";
            }
        }
    }
}
