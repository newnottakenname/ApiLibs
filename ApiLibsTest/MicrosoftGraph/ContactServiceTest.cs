﻿using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.MicrosoftGraph;
using NUnit.Framework;

namespace ApiLibsTest.MicrosoftGraph
{
    class ContactServiceTest
    {
        private PeopleService contacts;

        [OneTimeSetUp]
        public void SetUp()
        {
            contacts = GraphTest.GetGraphService().PeopleService;
        }

        [Test]
        public async Task GetContacts()
        {
            var contacts = await this.contacts.GetAllContacts();
        }

        [Test]
        public async Task GetContactsFromSearch()
        {
            var contacts = await this.contacts.GetContacts("Meneer");
        }
    }
}