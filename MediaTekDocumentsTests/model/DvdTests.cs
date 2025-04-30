using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class DvdTests
	{
        private const string id = "20004";
        private const string titre = "Pirates des Caraïbes";
        private const string image = "";
        private const string idGenre = "10019";
        private const string genre = "Fantazy";
        private const string idPublic = "00004";
        private const string lepublic = "Ados";
        private const string idRayon = "DF001";
        private const string rayon = "Dvd";
        private const string realisateur = "Gore Verbinski";
        private const string synopsis = "un pirate enleve une jeune fille afin de briser la malédiction de son bateau";
        private const int duree = 143;
        private static readonly Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, lepublic, idRayon, rayon);

        [TestMethod()]
		public void DvdTest()
		{
            Assert.AreEqual(id, dvd.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, dvd.Titre, "devrait réussor : titre valorisé");
            Assert.AreEqual(image, dvd.Image, "devrait réussir : image valorisée");
            Assert.AreEqual(idGenre, dvd.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, dvd.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, dvd.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(lepublic, dvd.Public, "devrait réussir : lepublic valorisé");
            Assert.AreEqual(idRayon, dvd.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, dvd.Rayon, "devrait réussir : rayon valorisé");
            Assert.AreEqual(realisateur, dvd.Realisateur, "devrait réussir : realisateur valorisé");
            Assert.AreEqual(synopsis, dvd.Synopsis, "devrait réussir : synopsis valorisé");
            Assert.AreEqual(duree, dvd.Duree, "devrait réussir : duree valorisée");
        }
	}
}
