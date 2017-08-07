using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ApiLibs;
using ApiLibs.General;

namespace ApiLibsTest.General
{
    class ObjectSearcherTest
    {
        private A a;
        private B b;
        private C c;
        private D d;

        private class Serv : Service
        {
            public Serv(string hostUrl) : base(hostUrl)
            {
            }
        }

        private class A : ObjectSearcher
        {
            public B b;
            public C c;
        }

        private class B : ObjectSearcher
        {
            public D d;
        }

        private class C { }

        private class D : ObjectSearcher { }

        [SetUp]
        public void SetUp()
        {
            d = new D();
            c = new C();
            b = new B();
            b.d = d;
            a = new A();
            a.b = b;
            a.c = new C();
        }

        [Test]
        public void FirstLevelTest()
        {
            Serv serv = new Serv("https://www.example.com");
            b.Search(serv);

            Assert.IsTrue(b.service == serv);
        }

        [Test]
        public void SecondLevelTest()
        {
            Serv serv = new Serv("https://www.example.com");
            b.Search(serv);

            Assert.IsTrue(d.service == serv);
        }
    }
}
