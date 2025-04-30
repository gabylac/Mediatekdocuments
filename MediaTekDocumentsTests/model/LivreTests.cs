using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class LivreTests
	{
        private const string id = "00015";
        private const string titre = "Conte de fées";
        private const string image = "";
        private const string idGenre = "10019";
        private const string genre = "Fantazy";
        private const string idPublic = "00004";
        private const string lepublic = "Ados";
        private const string idRayon = "LV0001";
        private const string rayon = "Littérature étrangère";
        private const string isbn = "monisbn";
        private const string auteur = "Stephen King";
        private const string collection = "";
        private static readonly Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, lepublic, idRayon, rayon);

        [TestMethod()]
		public void LivreTest()
		{
            Assert.AreEqual(id, livre.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, livre.Titre, "devrait réussor : titre valorisé");
            Assert.AreEqual(image, livre.Image, "devrait réussir : image valorisée");
            Assert.AreEqual(idGenre, livre.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, livre.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, livre.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(lepublic, livre.Public, "devrait réussir : lepublic valorisé");
            Assert.AreEqual(idRayon, livre.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, livre.Rayon, "devrait réussir : rayon valorisé");
            Assert.AreEqual(isbn, livre.Isbn, "devrait réussir : isbn valorisé");
            Assert.AreEqual(auteur, livre.Auteur, "devrait réussir : auteur valorisé");
            Assert.AreEqual(collection, livre.Collection, "devrait réussir : collection valorisée");
        }
	}
}
