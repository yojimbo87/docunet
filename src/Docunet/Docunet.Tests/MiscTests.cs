using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Docunet;

namespace Docunet.Tests
{
    [TestFixture()]
    public class MiscTests
    {
        [Test()]
        public void Should_clone_document_and_change_values_of_new_one()
        {
            var document1 = new Docunet()
                .String("foo1", "test string value 1")
                .String("foo2", "test string value 2")
                .String("bar.baz1", "test value string 1")
                .String("bar.baz2", "test value string 2");
            
            var document2 = document1.Clone();
            document2.String("foo2", "another value of test string 2");
            document2.String("bar.baz2", "another test string value 2");
            
            Assert.AreEqual("test string value 1", document1.String("foo1"));
            Assert.AreEqual("test string value 2", document1.String("foo2"));
            Assert.AreEqual("test value string 1", document1.String("bar.baz1"));
            Assert.AreEqual("test value string 2", document1.String("bar.baz2"));
            
            Assert.AreEqual("test string value 1", document2.String("foo1"));
            Assert.AreEqual("another value of test string 2", document2.String("foo2"));
            Assert.AreEqual("test value string 1", document2.String("bar.baz1"));
            Assert.AreEqual("another test string value 2", document2.String("bar.baz2"));
        }
        
        [Test()]
        public void Should_retrieve_new_document_except_specific_fields()
        {
            var document1 = new Docunet()
                .String("foo1", "test string value 1")
                .String("foo2", "test string value 2")
                .String("bar.baz1", "test value string 1")
                .String("bar.baz2", "test value string 2");
            
            var document2 = document1.Except("foo2", "bar.baz2");
            
            Assert.AreEqual("test string value 1", document1.String("foo1"));
            Assert.AreEqual("test string value 2", document1.String("foo2"));
            Assert.AreEqual("test value string 1", document1.String("bar.baz1"));
            Assert.AreEqual("test value string 2", document1.String("bar.baz2"));
            
            Assert.AreEqual(document1.String("foo1"), document2.String("foo1"));
            Assert.AreEqual(null, document2.String("foo2"));
            Assert.AreEqual(document1.String("bar.baz1"), document2.String("bar.baz1"));
            Assert.AreEqual(null, document2.String("bar.baz2"));
        }
        
        [Test()]
        public void Should_retrieve_new_document_only_with_specific_fields()
        {
            var document1 = new Docunet()
                .String("foo1", "test string value 1")
                .String("foo2", "test string value 2")
                .String("bar.baz1", "test value string 1")
                .String("bar.baz2", "test value string 2");
            
            var document2 = document1.Only("foo1", "bar.baz1");
            
            Assert.AreEqual("test string value 1", document1.String("foo1"));
            Assert.AreEqual("test string value 2", document1.String("foo2"));
            Assert.AreEqual("test value string 1", document1.String("bar.baz1"));
            Assert.AreEqual("test value string 2", document1.String("bar.baz2"));
            
            Assert.AreEqual(document1.String("foo1"), document2.String("foo1"));
            Assert.AreEqual(null, document2.String("foo2"));
            Assert.AreEqual(document1.String("bar.baz1"), document2.String("bar.baz1"));
            Assert.AreEqual(null, document2.String("bar.baz2"));
        }
    }
}
