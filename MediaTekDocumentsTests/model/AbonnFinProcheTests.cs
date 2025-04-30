using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class AbonnFinProcheTests
	{
		private const string titreRevue = "Le Canard enchaîné";
		private static DateTime dateFin = DateTime.Parse("2025-07-15 00:00:00", CultureInfo.InvariantCulture);
		private static readonly AbonnFinProche aboFinProche = new AbonnFinProche(titreRevue, dateFin);

		[TestMethod()]
		public void AbonnFinProcheTest()
		{
			Assert.AreEqual(titreRevue, aboFinProche.TitreRevue, "devrait réussir : titreRevue valorisé");
			Assert.AreEqual(dateFin, aboFinProche.DateFin, "devrait réussir : dateFin valorisée");
		}
	}
}
