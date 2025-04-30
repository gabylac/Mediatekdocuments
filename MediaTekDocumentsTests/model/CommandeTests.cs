using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class CommandeTests
	{
		private const string id = "00025";
		private static DateTime dateCom = DateTime.Parse("2025-04-26 00:00:00", CultureInfo.InvariantCulture);
		private const double montant = 12.5;
		private static readonly Commande com = new Commande(id, dateCom, montant);

		[TestMethod()]
		public void CommandeTest()
		{
			Assert.AreEqual(id, com.Id, "devrait réussir : id valorisé");
			Assert.AreEqual(dateCom, com.DateCommande, "devrait réussir : dateCom vamorisée");
			Assert.AreEqual(montant, com.Montant, "devrait réussir : montant valorisé");
		}
	}
}
