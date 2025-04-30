using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class ServiceTests
	{
        private const string id = "1";
        private const string libelle = "admin";
        private static readonly Service service = new Service(id, libelle);

        [TestMethod()]
		public void ServiceTest()
		{
            Assert.AreEqual(id, service.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, service.Libelle, "devrait réussir : libelle valorisé");
        }
	}
}
