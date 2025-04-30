using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class AbonnementTests
	{
        private const string id = "00452";
        private static DateTime dateCom = DateTime.Parse("2025-04-26 00:00:00", CultureInfo.InvariantCulture);
        private const double montant = 8.3;
		private static DateTime dateFinAb = DateTime.Parse("2025-06-25 00:00:00", CultureInfo.InvariantCulture);
		private const string idRevue = "50006";
		private static Abonnement abo = new Abonnement(dateFinAb, idRevue, id, dateCom, montant);
        [TestMethod()]
		public void AbonnementTest()
		{
            Assert.AreEqual(id, abo.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCom, abo.DateCommande, "devrait réussir : dateCom vamorisée");
            Assert.AreEqual(montant, abo.Montant, "devrait réussir : montant valorisé");
			Assert.AreEqual(idRevue, abo.IdRevue, "devrait réussir : idRevue valorisé");
			Assert.AreEqual(dateFinAb, abo.DateFinAbonnement, "devrait réussir : dateFinAb valorisée");
        }
	}
}
