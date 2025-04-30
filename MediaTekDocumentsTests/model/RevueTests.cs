using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class RevueTests
	{
		private const string periodicite = "MS";
		private const int delaiMiseADispo = 52;
        private const string id = "00015";
        private const string titre = "Conte de fées";
        private const string image = "";
        private const string idGenre = "10019";
        private const string genre = "Fantazy";
        private const string idPublic = "00004";
        private const string lepublic = "Ados";
        private const string idRayon = "LV0001";
        private const string rayon = "Littérature étrangère";
        private static readonly Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, lepublic, idRayon, rayon, periodicite, delaiMiseADispo);

		[TestMethod()]
		public void RevueTest()
		{
            Assert.AreEqual(id, revue.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, revue.Titre, "devrait réussor : titre valorisé");
            Assert.AreEqual(image, revue.Image, "devrait réussir : image valorisée");
            Assert.AreEqual(idGenre, revue.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, revue.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, revue.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(lepublic, revue.Public, "devrait réussir : lepublic valorisé");
            Assert.AreEqual(idRayon, revue.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, revue.Rayon, "devrait réussir : rayon valorisé");
            Assert.AreEqual(periodicite, revue.Periodicite, "devrait réussir : periodicite valorisée");
            Assert.AreEqual(delaiMiseADispo, revue.DelaiMiseADispo, "devriat réussir : delaiMiseADispo valorisé");
        }
	}
}
