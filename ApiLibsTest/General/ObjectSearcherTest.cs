using System;
using System.Collections.Generic;
using System.Text;
using ApiLibs;
using ApiLibs.General;
using NUnit.Framework;

namespace ApiLibsTest.General
{
    public class ObjectSearcherTest
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
            public B b { get; set; }
            public C c { get; set; }
        }

        private class B : ObjectSearcher
        {
            public D d { get; set; }
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
            a.Search(serv);

            Assert.IsTrue(a.service == serv);
        }

        [Test]
        public void SecondLevelTest()
        {
            Serv serv = new Serv("https://www.example.com");
            a.Search(serv);

            Assert.IsTrue(b.service == serv);
        }

        [Test]
        public void ThirdLevelTest()
        {
            Serv serv = new Serv("https://www.example.com");
            a.Search(serv);

            Assert.IsTrue(b.service == serv);
        }
    }
}
