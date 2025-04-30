using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class SuiviTests
	{
		private const string id = "1";
		private const string libelle = "en cours";
		private static readonly Suivi sui = new Suivi(id, libelle);

		[TestMethod()]
		public void SuiviTest()
		{
			Assert.AreEqual(id, sui.Id, "devrait réussir : id valorisé");
			Assert.AreEqual(libelle, sui.Libelle, "devrait réussir : libelle valorisé");
		}
	}
}
