using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class DocumentTests
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
        private static readonly Document doc = new Document(id, titre, image, idGenre, genre, idPublic, lepublic, idRayon, rayon);

        [TestMethod()]
        public void DocumentTest()
        {
            Assert.AreEqual(id, doc.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, doc.Titre, "devrait réussor : titre valorisé");
            Assert.AreEqual(image, doc.Image, "devrait réussir : image valorisée");
            Assert.AreEqual(idGenre, doc.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, doc.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, doc.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(lepublic, doc.Public, "devrait réussir : lepublic valorisé");
            Assert.AreEqual(idRayon, doc.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, doc.Rayon, "devrait réussir : rayon valorisé");
        }
    }
}