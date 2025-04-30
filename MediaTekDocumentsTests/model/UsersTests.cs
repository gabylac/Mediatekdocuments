using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class UsersTests
	{
		private const string id = "00010";
		private const string login = "magicien";
		private const string idService = "3";
		private static Users user = new Users(id, login, idService);

		[TestMethod()]
		public void UsersTest()
		{
			Assert.AreEqual(id, user.Id, "devrait réussir : id valorisé");
			Assert.AreEqual(login, user.Login, "devrait réussir : login valorisé");
			Assert.AreEqual(idService, user.IdService, "devrait réussir : idService valorisé");
		}
	}
}
