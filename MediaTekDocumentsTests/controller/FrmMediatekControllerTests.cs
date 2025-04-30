using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.controller.Tests
{
	[TestClass()]
	public class FrmMediatekControllerTests
	{
		private static readonly FrmMediatekController controller = new FrmMediatekController();
		[TestMethod()]
		public void ParutionDansAbonnementTest()
		{
			DateTime dateCom = DateTime.Parse("2025-02-23 12:00:00", CultureInfo.InvariantCulture);
			DateTime datePar = DateTime.Parse("2025-03-15 12:00:00", CultureInfo.InvariantCulture);
			DateTime dateFinAb = DateTime.Parse("2025-05-30 00:00:00", CultureInfo.InvariantCulture);
			Assert.IsTrue(controller.ParutionDansAbonnement(dateCom, dateFinAb, datePar),
				"devrait réussir : datePar entre dateCom et dateFinAb");
			DateTime dateParAnt = DateTime.Parse("2025-01-10 00:00:00", CultureInfo.InvariantCulture);
			Assert.IsFalse(controller.ParutionDansAbonnement(dateCom, dateFinAb, dateParAnt),
				"devrait echouer : dateParFalse antérieure à dateCom");
			DateTime dateParPost = DateTime.Parse("2025-06-15 00:00:00", CultureInfo.InvariantCulture);
			Assert.IsFalse(controller.ParutionDansAbonnement(dateCom, dateFinAb, dateParPost),
				"devrait échouer : dateParPost posterieure à dateFinAb");
		}
	}
}
