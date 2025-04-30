using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class PublicTests
	{
        private const string id = "blabla";
        private const string libelle = "enfant";
        private static readonly Public lepublic = new Public(id, libelle);

        [TestMethod()]
		public void PublicTest()
		{
            Assert.AreEqual(id, lepublic.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, lepublic.Libelle, "devrait réussir : libelle valorisé");
        }
	}
}
