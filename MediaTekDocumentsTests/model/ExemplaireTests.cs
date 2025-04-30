using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class ExemplaireTests
	{
		private const string id = "12345";
		private const int numero = 345;
		private const string photo = "";
		private static DateTime dateAchat = DateTime.Parse("2025-04-25 00:00:00", CultureInfo.InvariantCulture);
		private const string idEtat = "poubelle";
		private static readonly Exemplaire ex = new Exemplaire(numero, dateAchat, photo, idEtat, id);

		[TestMethod()]
		public void ExemplaireTest()
		{
            Assert.AreEqual(id, ex.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(numero, ex.Numero, "devrait réussir : numero valorisé");
            Assert.AreEqual(photo, ex.Photo, "devrait réussir : photo valorisée");
            Assert.AreEqual(dateAchat, ex.DateAchat, "devrait réussir : dateAchat valorisée");
			Assert.AreEqual(idEtat, ex.IdEtat, "devrait réussir : idEtat valorisé");
        }
	}
}
