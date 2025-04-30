using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();

        private readonly BindingSource bdgSuivis = new BindingSource();
        private List<Suivi> lesSuivis = new List<Suivi>();

        public GroupBox groupBoxRecExemplaire => grpReceptionExemplaire;
        public TabPage tabComLivres => tabComLivre;
        public TabPage tabComDVD => tabComDvd;
        public TabPage tabAbonnement => tabAbonnementRevue;

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        static public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// rempli le combo suivi
        /// </summary>
        public void ChargerComboSuivi()
        {
            lesSuivis = controller.GetAllSuivis();
            bdgSuivis.DataSource = lesSuivis;
            cbxStatut.DataSource = bdgSuivis;
            cbxModifStatDvd.DataSource = bdgSuivis;
            if (cbxStatut.Items.Count > 0 && cbxModifStatDvd.Items.Count > 0)
            {
                cbxStatut.SelectedIndex = -1;
                cbxModifStatDvd.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// création du nouvel id commande à partir du dernier
        /// </summary>
        /// <returns>id nouvelle commande</returns>
        public string NewIdCommande()
        {
            List<Commande> lesCommandes = controller.GetAllCommandes();
            Commande lastCommande = lesCommandes.LastOrDefault();
            string lastIdCommande = lastCommande.Id;
            int lastIdCom = int.Parse(lastIdCommande);
            lastIdCom++;
            string idNewCom = lastIdCom.ToString("D5");
            return idNewCom;
        }

        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    Log.Information("Erreur lors de la saisie du numéro de document(introuvable). numero={0}", txbLivresNumRecherche.Text);
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    Log.Information("Erreur lors de la saisie du numéro de document(introuvable). numero={0}", txbDvdNumRecherche.Text);
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    Log.Information("Erreur lode la saisie du numero de document(introuvable). numero={0}", txbRevuesNumRecherche.Text);
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    Log.Information("erreur lors de la saisie du numéro de document(introuvable). numero={0}", txbReceptionRevueNumero.Text);
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        Log.Information("Erreur lors de la saisie du numéro de l'exemplaire, numero déjà existant. numero={0}", numero);
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    Log.Information("Erreur de conversion en int du numéro saisi. numero={0}", txbReceptionExemplaireNumero.Text);
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                Log.Information("Erreur de saisie du numéro de l'exemplaire, le champ ne doit pas être vide. numero={0}", txbReceptionExemplaireNumero.Text);
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion

        #region onglet CommandeLivre
        private readonly BindingSource bdgCommandesLivreListe = new BindingSource();
        private List<CommandeDocument> lesCommandesLivres = new List<CommandeDocument>();

        /// <summary>
        /// ouverture de l'onglet commande livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabComLivre_Enter(object sender, EventArgs e)
        {
            ChargerComboSuivi();
            txtbLivreRechercheNum.Text = "";
            ViderZoneInfoLivre();
        }

        /// <summary>
        /// vide les zones d'affichage des infos du livre
        /// </summary>
        private void ViderZoneInfoLivre()
        {
            txtbLivreTitre.Text = "";
            txtbLivreRayon.Text = "";
            txtbLivrePublic.Text = "";
            txtbLivreIsbn.Text = "";
            txtbLivreImage.Text = "";
            txtbLivreGenre.Text = "";
            txtbLivreCollection.Text = "";
            txtbLivreAuteur.Text = "";
            dgvListeComLivre.DataSource = "";
        }

        /// <summary>
        /// recherche d'un livre suite à la saisie de son numéro
        /// affichage des informations concernant le livre et ses commandes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivreRechercheNum_Click(object sender, EventArgs e)
        {
            if (!txtbLivreRechercheNum.Text.Equals(""))
            {
                Livre livre = lesLivres.Find(x => x.Id.Equals(txtbLivreRechercheNum.Text));
                if (livre != null)
                {
                    AfficheCommandesListe();
                    AfficheLivresDetails(livre);
                }
                else
                {
                    Log.Information("Erreur lors de la saisie du numero du document(introuvable). numero={0}", txtbLivreRechercheNum.Text);
                    MessageBox.Show("le numéro saisi n'existe pas");
                    txtbLivreRechercheNum.Text = "";
                }

            }
            else
            {
                Log.Information("Erreur le champ du numero de document est vide. numero={0}", txtbLivreRechercheNum.Text);
                MessageBox.Show("saisir un numéro de livre");
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresDetails(Livre livre)
        {
            //affiche les infos du livre
            txtbLivreAuteur.Text = livre.Auteur;
            txtbLivreCollection.Text = livre.Collection;
            txtbLivreImage.Text = livre.Image;
            txtbLivreIsbn.Text = livre.Isbn;
            txtbLivreGenre.Text = livre.Genre;
            txtbLivrePublic.Text = livre.Public;
            txtbLivreRayon.Text = livre.Rayon;
            txtbLivreTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivreImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivreImage.Image = null;
            }

        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesCommandesLivre">liste de commmandes</param>
        private void RemplirCommandesLivreListe(List<CommandeDocument> lesCommandesLivre)
        {
            if (lesCommandesLivre != null)
            {
                bdgCommandesLivreListe.DataSource = lesCommandesLivre;
                dgvListeComLivre.DataSource = bdgCommandesLivreListe;
                dgvListeComLivre.Columns["idSuivi"].Visible = false;
                dgvListeComLivre.Columns["idLivreDvd"].Visible = false;
                dgvListeComLivre.Columns["id"].Visible = false;
                dgvListeComLivre.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvListeComLivre.Columns["dateCommande"].DisplayIndex = 0;
                dgvListeComLivre.Columns["montant"].DisplayIndex = 2;
            }
            else
            {
                bdgCommandesLivreListe.DataSource = null;
            }

        }

        /// <summary>
        /// récupère et affiche les commandes d'un livre
        /// </summary>
        public void AfficheCommandesListe()
        {
            string idLivreDvd = txtbLivreRechercheNum.Text;
            lesCommandesLivres = controller.GetCommandesDocument(idLivreDvd);
            RemplirCommandesLivreListe(lesCommandesLivres);
        }

        /// <summary>
        /// tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeComLivre_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvListeComLivre.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Suivi":
                    sortedList = lesCommandesLivres.OrderBy(o => o.Suivi).Reverse().ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandesLivres.OrderBy(o => o.DateCommande).Reverse().ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesLivres.OrderBy(o => o.Montant).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = lesCommandesLivres.OrderBy(o => o.NbExemplaire).ToList();
                    break;
            }
            RemplirCommandesLivreListe(sortedList);
        }

        /// <summary>
        /// ajoute une nouvelle commande pour un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCommande_Click(object sender, EventArgs e)
        {
            if (!txtbLivreRechercheNum.Text.Equals(""))
            {
                try
                {
                    //création de l'id de la nouvelle commande                    
                    string id = NewIdCommande();

                    DateTime dateCommande = dtpDateCommande.Value;
                    double montant = double.Parse(txtbMontant.Text);

                    int nbExemplaire = int.Parse(txtbNbExemplaires.Text);
                    string idSuivi = "1";
                    string Suivi = "en cours";
                    string idLivreDvd = txtbLivreRechercheNum.Text;

                    CommandeDocument commande = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, Suivi);
                    if (controller.CreerCommandeDocument(commande))
                    {
                        AfficheCommandesListe();
                        txtbMontant.Text = "";
                        txtbNbExemplaires.Text = "";
                    }
                    else
                    {
                        Log.Information("Erreur lors de l'execution de la methode CreerCommandeDocument");
                        MessageBox.Show("création commande échouée", "Erreur");
                    }
                }
                catch
                {
                    Log.Information("Erreur de conversion des champs 'nombre exempalires' et 'montant'");
                    MessageBox.Show("le nombre d'exemplaires et le montant doivent être numériques", "Information");
                }
            }
            else
            {
                Log.Information("Erreur, le champ numero de document est vide. numero={0}", txtbLivreRechercheNum.Text);
                MessageBox.Show("numéro de document obligatoire", "Information");
            }

        }

        /// <summary>
        /// modifie le statut de la commande selectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveStatut_Click(object sender, EventArgs e)
        {
            string newSuivi = cbxStatut.Text;
            lesSuivis = controller.GetAllSuivis();
            bdgSuivis.DataSource = lesSuivis;
            string idNewSuivi = "";
            idNewSuivi = lesSuivis.FirstOrDefault(suivi => suivi.Libelle.Equals(newSuivi, StringComparison.OrdinalIgnoreCase))?.Id;

            if (dgvListeComLivre.CurrentCell != null)
            {
                CommandeDocument commande = (CommandeDocument)bdgCommandesLivreListe.List[bdgCommandesLivreListe.Position];
                if ((commande.Suivi == "en cours" || commande.Suivi == "relancée") && cbxStatut.Text != "réglée")
                {

                    commande.Suivi = newSuivi;
                    commande.IdSuivi = idNewSuivi;
                    controller.UpdateCommandeDocument(commande, commande.Id);
                    AfficheCommandesListe();
                }
                else
                {
                    if (commande.Suivi == "livrée" && (cbxStatut.Text != "en cours" || cbxStatut.Text != "relancée"))
                    {
                        commande.Suivi = newSuivi;
                        commande.IdSuivi = idNewSuivi;
                        controller.UpdateCommandeDocument(commande, commande.Id);
                        AfficheCommandesListe();
                    }
                }

            }
        }

        /// <summary>
        /// supprime une commande d'un document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSuppCommande_Click(object sender, EventArgs e)
        {
            CommandeDocument commandeDoc = (CommandeDocument)bdgCommandesLivreListe.List[bdgCommandesLivreListe.Position];
            string idComSupp = commandeDoc.Id;
            if (!commandeDoc.Suivi.Equals("livrée"))
            {
                controller.DeleteCommande(idComSupp);
                AfficheCommandesListe();
            }

        }
        #endregion

        #region onglet CommandeDvd
        private readonly BindingSource bdgCommandesDvdListe = new BindingSource();
        private List<CommandeDocument> lesCommandesDvd = new List<CommandeDocument>();

        /// <summary>
        /// ouverture de l'onglet commande DVD
        /// appel de la méthode qui remplit le combo de suivi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabComDvd_Enter(object sender, EventArgs e)
        {
            ChargerComboSuivi();
            txtbRechercheNum.Text = "";
            ViderZoneInfoDvd();
        }

        /// <summary>
        /// vide les zones d'affichage des infos du Dvd
        /// </summary>
        private void ViderZoneInfoDvd()
        {
            txtbCheminImDvd.Text = "";
            txtbDureeDvd.Text = "";
            txtbGenreDvd.Text = "";
            txtbPublicDvd.Text = "";
            txtbRayonDvd.Text = "";
            txtbRealisateurDvd.Text = "";
            txtbSynopsisDvd.Text = "";
            txtbTitreDvd.Text = "";
            dgvCommandeDvd.DataSource = "";
        }

        /// <summary>
        /// recherche d'un dvd suite à la saisie de son numéro
        /// affichage de ses informations et de ses commandes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRech_Click(object sender, EventArgs e)
        {
            if (!txtbRechercheNum.Text.Equals(""))
            {
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txtbRechercheNum.Text));
                if (dvd != null)
                {
                    AfficheCommandesListeDvd();
                    AfficheDvdDetails(dvd);
                }
                else
                {
                    Log.Information("Erreur lors de la saisie du numero du document(introuvable). numero={0}", txtbRechercheNum.Text);
                    MessageBox.Show("le numéro saisi n'existe pas");
                    txtbLivreRechercheNum.Text = "";
                }

            }
            else
            {
                Log.Information("Erreur, le champ numero de document est vide. numero={0}", txtbRechercheNum.Text);
                MessageBox.Show("saisir un numéro de dvd");
            }
        }

        /// <summary>
        /// affiche les détails d'un DVD selectionné
        /// </summary>
        /// <param name="dvd"></param>
        private void AfficheDvdDetails(Dvd dvd)
        {
            //affiche les infos du dvd
            txtbRealisateurDvd.Text = dvd.Realisateur;
            txtbDureeDvd.Text = dvd.Duree.ToString();
            txtbSynopsisDvd.Text = dvd.Synopsis;
            txtbPublicDvd.Text = dvd.Public;
            txtbRayonDvd.Text = dvd.Rayon;
            txtbTitreDvd.Text = dvd.Titre;
            txtbCheminImDvd.Text = dvd.Image;
            txtbGenreDvd.Text = dvd.Genre;
            string image = dvd.Image;
            try
            {
                pcbLivreImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivreImage.Image = null;
            }

        }

        /// <summary>
        /// remplit le datagrid avec la liste reçue en paramètr
        /// </summary>
        /// <param name="lesCommandesDvd">liste des commandes</param>
        private void RemplirCommandeDvdListe(List<CommandeDocument> lesCommandesDvd)
        {
            if (lesCommandesDvd != null)
            {
                bdgCommandesDvdListe.DataSource = lesCommandesDvd;
                dgvCommandeDvd.DataSource = bdgCommandesDvdListe;
                dgvCommandeDvd.Columns["idSuivi"].Visible = false;
                dgvCommandeDvd.Columns["idLivreDvd"].Visible = false;
                dgvCommandeDvd.Columns["id"].Visible = false;
                dgvCommandeDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCommandeDvd.Columns["dateCommande"].DisplayIndex = 0;
                dgvCommandeDvd.Columns["montant"].DisplayIndex = 2;
            }
            else
            {
                bdgCommandesDvdListe.DataSource = null;
            }
        }

        public void AfficheCommandesListeDvd()
        {
            string idLivreDvd = txtbRechercheNum.Text;
            lesCommandesDvd = controller.GetCommandesDocument(idLivreDvd);
            RemplirCommandeDvdListe(lesCommandesDvd);
        }

        /// <summary>
        /// supprime une commande de la liste des commandes d'un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSuppComDvd_Click(object sender, EventArgs e)
        {
            CommandeDocument commandeDoc = (CommandeDocument)bdgCommandesDvdListe.List[bdgCommandesDvdListe.Position];
            string idComSupp = commandeDoc.Id;
            if (!commandeDoc.Suivi.Equals("livrée"))
            {
                controller.DeleteCommande(idComSupp);
                AfficheCommandesListeDvd();
            }
        }

        /// <summary>
        /// ajoute une commande à la liste des commandes d'un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnrgComDvd_Click(object sender, EventArgs e)
        {
            if (!txtbRechercheNum.Text.Equals(""))
            {
                try
                {
                    //création de l'id de la nouvelle commande                    
                    string id = NewIdCommande();

                    DateTime dateCommande = dtpComDvd.Value;
                    double montant = double.Parse(txtbMontantDvd.Text);

                    int nbExemplaire = int.Parse(txtbNbExDvd.Text);
                    string idSuivi = "1";
                    string Suivi = "en cours";
                    string idLivreDvd = txtbRechercheNum.Text;

                    CommandeDocument commande = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, Suivi);
                    if (controller.CreerCommandeDocument(commande))
                    {
                        AfficheCommandesListeDvd();
                        txtbMontant.Text = "";
                        txtbNbExemplaires.Text = "";
                    }
                    else
                    {
                        Log.Information("Erreur lors de l'execution de la méthode CreerCommandeDocument");
                        MessageBox.Show("création commande échouée", "Erreur");
                    }
                }
                catch
                {
                    Log.Information("Erreur de conversion des champs 'nombre exemplaires' et 'montant'");
                    MessageBox.Show("le nombre d'exemplaires et le montant doivent être numériques", "Information");
                }
            }
            else
            {
                Log.Information("Erreur, le champ numero de document est vide. numero={0}", txtbRechercheNum.Text);
                MessageBox.Show("numéro de document obligatoire", "Information");
            }
        }

        /// <summary>
        /// modifie le statut de la commande selectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifStatDvd_Click(object sender, EventArgs e)
        {
            string newSuivi = cbxModifStatDvd.Text;
            lesSuivis = controller.GetAllSuivis();
            bdgSuivis.DataSource = lesSuivis;
            string idNewSuivi = "";
            idNewSuivi = lesSuivis.FirstOrDefault(suivi => suivi.Libelle.Equals(newSuivi, StringComparison.OrdinalIgnoreCase))?.Id;

            if (dgvCommandeDvd.CurrentCell != null)
            {
                CommandeDocument commande = (CommandeDocument)bdgCommandesDvdListe.List[bdgCommandesDvdListe.Position];
                if ((commande.Suivi == "en cours" || commande.Suivi == "relancée") && cbxStatut.Text != "réglée")
                {

                    commande.Suivi = newSuivi;
                    commande.IdSuivi = idNewSuivi;
                    controller.UpdateCommandeDocument(commande, commande.Id);
                    AfficheCommandesListeDvd();
                }
                else
                {
                    if (commande.Suivi == "livrée" && (cbxStatut.Text != "en cours" || cbxStatut.Text != "relancée"))
                    {
                        commande.Suivi = newSuivi;
                        commande.IdSuivi = idNewSuivi;
                        controller.UpdateCommandeDocument(commande, commande.Id);
                        AfficheCommandesListeDvd();
                    }
                }

            }
        }

        /// <summary>
        /// tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandeDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandeDvd.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Suivi":
                    sortedList = lesCommandesDvd.OrderBy(o => o.Suivi).Reverse().ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandesDvd.OrderBy(o => o.DateCommande).Reverse().ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDvd.OrderBy(o => o.Montant).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = lesCommandesDvd.OrderBy(o => o.NbExemplaire).ToList();
                    break;
            }
            RemplirCommandeDvdListe(sortedList);
        }

        #endregion

        #region AbonnementRevue
        private readonly BindingSource bdgAbonnementRevueListe = new BindingSource();
        private List<Abonnement> lesAbonnementsRevue = new List<Abonnement>();

        /// <summary>
        /// ouverture de l'onglet abonnement revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabAbonnementRevue_Enter(object sender, EventArgs e)
        {
            txtbNumRechRevue.Text = "";
            VideZoneInfoRevue();
        }

        /// <summary>
        /// vide les zones d'affichage des infos de la revue
        /// </summary>
        private void VideZoneInfoRevue()
        {
            txtbTitreRevue.Text = "";
            txtbRayonRevue.Text = "";
            txtbPublicRevue.Text = "";
            txtbPeriodiciteRevue.Text = "";
            txtbGenreRevue.Text = "";
            txtbDelaiDispoRevue.Text = "";
            txtbCheminImRevue.Text = "";
            dgvListeAbonnRevue.DataSource = "";
        }

        /// <summary>
        /// recherche d'une revue suite à la saisie de son numéro
        /// affichage des informations et des commandes de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechNumRevue_Click(object sender, EventArgs e)
        {
            if (!txtbNumRechRevue.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txtbNumRechRevue.Text));
                if (revue != null)
                {
                    AfficheAbonnementsListe();
                    AfficheRevueDetails(revue);
                }
                else
                {
                    MessageBox.Show("le numéro saisi n'existe pas");
                    txtbNumRechRevue.Text = "";
                }

            }
            else
            {
                Log.Information("Erreur, le champ numero de document est vide. numero={0}", txtbNumRechRevue.Text);
                MessageBox.Show("saisir un numéro de revue");
            }
        }

        /// <summary>
        /// affiche les détails de la revue reçue en paramètre
        /// </summary>
        /// <param name="revue">objet revue concerné</param>
        private void AfficheRevueDetails(Revue revue)
        {
            //affiche les détails de la revue
            txtbTitreRevue.Text = revue.Titre;
            txtbPeriodiciteRevue.Text = revue.Periodicite;
            txtbDelaiDispoRevue.Text = revue.DelaiMiseADispo.ToString();
            txtbGenreRevue.Text = revue.Genre;
            txtbPublicRevue.Text = revue.Public;
            txtbRayonRevue.Text = revue.Rayon;
            txtbCheminImRevue.Text = revue.Image;
            string image = revue.Image;
            try
            {
                pctbImRevue.Image = Image.FromFile(image);
            }
            catch
            {
                pctbImRevue.Image = null;
            }
        }

        /// <summary>
        /// récupère la liste des abonnements d'une revue
        /// </summary>
        public void AfficheAbonnementsListe()
        {
            string idRevue = txtbNumRechRevue.Text;
            lesAbonnementsRevue = controller.GetAbonnementsRevue(idRevue);
            RemplirAbonnementsListeRevue(lesAbonnementsRevue);
        }

        /// <summary>
        /// remplit le datagrid de la liste des abonnements d'une revue reçue en paramètre
        /// </summary>
        /// <param name="lesAbonnementsRevue">liste des abonnements de la revue concernée</param>
        public void RemplirAbonnementsListeRevue(List<Abonnement> lesAbonnementsRevue)
        {
            if (lesAbonnementsRevue != null)
            {
                bdgAbonnementRevueListe.DataSource = lesAbonnementsRevue;
                dgvListeAbonnRevue.DataSource = bdgAbonnementRevueListe;
                dgvListeAbonnRevue.Columns["id"].Visible = false;
                dgvListeAbonnRevue.Columns["idRevue"].Visible = false;
                dgvListeAbonnRevue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvListeAbonnRevue.Columns["dateCommande"].DisplayIndex = 0;
                dgvListeAbonnRevue.Columns["montant"].DisplayIndex = 2;
            }
            else
            {
                bdgCommandesLivreListe.DataSource = null;
            }
        }

        /// <summary>
        /// ajoute un abonnement à une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnrAbonnRevue_Click(object sender, EventArgs e)
        {
            if (!txtbNumRechRevue.Text.Equals(""))
            {
                try
                {
                    //création de l'id de la nouvelle commande                   
                    string id = NewIdCommande();

                    string idRevue = txtbNumRechRevue.Text;
                    DateTime dateCommande = dtpDateComRevue.Value;
                    DateTime dateFinAbonnement = dtpDateFinAbonn.Value;
                    double montant = double.Parse(txtbMontantAbonnRevue.Text);
                    Abonnement abonnement = new Abonnement(dateFinAbonnement, idRevue, id, dateCommande, montant);
                    if (controller.CreerAbonnementRevue(abonnement))
                    {
                        AfficheAbonnementsListe();
                        txtbMontantAbonnRevue.Text = "";
                        dtpDateFinAbonn.Value = DateTime.Now;
                        dtpDateComRevue.Value = DateTime.Now;
                    }
                }
                catch
                {
                    Log.Information("Erreur de conversion du montant en numérique. montant={0}", txtbMontantAbonnRevue.Text);
                    MessageBox.Show("le montant doit être numérique", "Information");
                }

            }
        }

        /// <summary>
        /// tri sur les titres des colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeAbonnRevue_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvListeComLivre.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "DateFinAbonnement":
                    sortedList = lesAbonnementsRevue.OrderBy(o => o.DateFinAbonnement).Reverse().ToList();
                    break;
                case "DateCommande":
                    sortedList = lesAbonnementsRevue.OrderBy(o => o.DateCommande).Reverse().ToList();
                    break;
                case "Montant":
                    sortedList = lesAbonnementsRevue.OrderBy(o => o.Montant).ToList();
                    break;
            }
            RemplirAbonnementsListeRevue(sortedList);
        }

        

        /// <summary>
        /// supprime la commande d'une revue à condition qu'aucun exemplaire n'y soit rattaché
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprAbonn_Click(object sender, EventArgs e)
        {
            Abonnement abonnementRevue = (Abonnement)bdgAbonnementRevueListe.List[bdgAbonnementRevueListe.Position];
            string idAbonnSupp = abonnementRevue.Id;
            string idRevue = txtbNumRechRevue.Text;
            lesExemplaires = controller.GetExemplairesRevue(idRevue);            

            bool okSuppr = !lesExemplaires.Any(ex => controller.ParutionDansAbonnement(abonnementRevue.DateCommande, abonnementRevue.DateFinAbonnement, ex.DateAchat));

            if (okSuppr)
            {
                controller.DeleteCommande(idAbonnSupp);
                AfficheAbonnementsListe();
            }
            else
            {
                Log.Information("la méthode 'parutionDansAbonnement' n'est pas vraie");
                MessageBox.Show("Impossible de supprimer l'abonnement, des exemlaires y sont rattachés");
            }

        }


        #endregion

        
    }
}
