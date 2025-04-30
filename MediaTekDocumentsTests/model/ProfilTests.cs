using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
	[TestClass()]
	public class ProfilTests
	{
        private const string login = "sorciere";
        private const string pwd = "pwdsorciere";
        private static readonly Profil profil = new Profil(login, pwd);

        [TestMethod()]
		public void ProfilTest()
		{
            Assert.AreEqual(login, profil.Login, "devrait réussir : login valorisé");
            Assert.AreEqual(pwd, profil.Pwd, "devrait réussir : pwd valorisé");
        }
	}
}
