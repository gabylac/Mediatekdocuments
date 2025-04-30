using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class RayonTests
	{
        private const string id = "1234";
        private const string libelle = "Voyages";
		private static readonly Rayon rayon = new Rayon(id, libelle);

        [TestMethod()]
		public void RayonTest()
		{
            Assert.AreEqual(id, rayon.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, rayon.Libelle, "devrait réussir : libelle valorisé");
        }
	}
}
