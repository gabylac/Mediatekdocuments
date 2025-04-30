using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class CommandeDocumentTests
	{
        private const string id = "00152";
        private static DateTime dateCom = DateTime.Parse("2025-04-26 00:00:00", CultureInfo.InvariantCulture);
        private const double montant = 25.8;
		private const string idDoc = "20005";
		private const int nbEx = 5;
		private const string idSuivi = "2";
		private const string suivi = "perdu";
		private static readonly CommandeDocument comDoc = new CommandeDocument(id, dateCom, montant, nbEx, idDoc, idSuivi, suivi);
        [TestMethod()]
		public void CommandeDocumentTest()
		{
            Assert.AreEqual(id, comDoc.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCom, comDoc.DateCommande, "devrait réussir : dateCom vamorisée");
            Assert.AreEqual(montant, comDoc.Montant, "devrait réussir : montant valorisé");
			Assert.AreEqual(nbEx, comDoc.NbExemplaire, "devrait réussir : nbEx valorisé");
			Assert.AreEqual(idDoc, comDoc.IdLivreDvd, "devrait réussir : idDoc valorisé");
			Assert.AreEqual(idSuivi, comDoc.IdSuivi, "devrait réussir : idSuivi valorisé");
			Assert.AreEqual(suivi, comDoc.Suivi, "devrait réussir : suivi valorisé");
        }
	}
}
