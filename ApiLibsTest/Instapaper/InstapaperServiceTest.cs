using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Instapaper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiLibsTest.Instapaper
{
    [TestClass]
    public class InstapaperServiceTest
    {
        private InstapaperService instapaper;

        [TestInitialize]
        public void Setup()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            instapaper = new InstapaperService(passwords.Instaper_Consumer_ID, passwords.Instaper_Consumer_Secret, passwords.Instaper_Access_Token, passwords.Instaper_Access_Token_Secret);
        }

        [TestMethod]
        public async Task ConnectTest()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            instapaper = new InstapaperService(passwords.Instaper_Consumer_ID, passwords.Instaper_Consumer_ID, passwords.Instaper_Access_Token, passwords.Instaper_Access_Token_Secret);
            await instapaper.Connect("email", "password", passwords.Instaper_Consumer_ID, passwords.Instaper_Consumer_Secret);
        }

        [TestMethod]
        public async Task GetFoldersTest()
        {
            //await instapaper.GetFolders();
            await instapaper.GetBookmarks(30);
        }
    }
}
