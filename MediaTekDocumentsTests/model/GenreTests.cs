using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class GenreTests
	{
        private const string id = "blabla";
        private const string libelle = "enfant";
        private static readonly Genre genre = new Genre(id, libelle);

        [TestMethod()]
		public void GenreTest()
		{
            Assert.AreEqual(id, genre.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, genre.Libelle, "devrait réussir : libelle valorisé");
        }
	}
}
