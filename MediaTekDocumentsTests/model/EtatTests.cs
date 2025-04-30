using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class EtatTests
	{
        private const string id = "00005";
        private const string libelle = "poubelle";
        private static readonly Etat etat = new Etat(id, libelle);
		
        [TestMethod()]
		public void EtatTest()
		{
            Assert.AreEqual(id, etat.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, etat.Libelle, "devrait réussir : libelle valorisé");
        }
	}
}
