using System;
using System.Collections.Generic;
using NUnit.Framework;
using Docunet;

namespace Docunet.Tests
{
    [TestFixture()]
    public class FieldTests
    {
        [Test()]
        public void Should_get_and_set_bool_values()
        {
            var document = new Docunet()
                .Bool("foo", true)
                .Bool("bar.baz", false);

            Assert.AreEqual(true, document.Bool("foo"));
            Assert.AreEqual(false, document.Bool("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_byte_values()
        {
            var document = new Docunet()
                .Byte("foo", 123)
                .Byte("bar.baz", 200);
            
            Assert.AreEqual(123, document.Byte("foo"));
            Assert.AreEqual(200, document.Byte("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_short_values()
        {
            var document = new Docunet()
                .Short("foo", 1234)
                .Short("bar.baz", 4321);
            
            Assert.AreEqual(1234, document.Short("foo"));
            Assert.AreEqual(4321, document.Short("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_int_values()
        {
            var document = new Docunet()
                .Int("foo", 123456)
                .Int("bar.baz", 654321);
            
            Assert.AreEqual(123456, document.Int("foo"));
            Assert.AreEqual(654321, document.Int("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_long_values()
        {
            var document = new Docunet()
                .Long("foo", 123456789012345)
                .Long("bar.baz", 543210987654321);
            
            Assert.AreEqual(123456789012345, document.Long("foo"));
            Assert.AreEqual(543210987654321, document.Long("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_float_values()
        {
            var document = new Docunet()
                .Float("foo", 123.456f)
                .Float("bar.baz", 654.321f);
            
            Assert.AreEqual(123.456f, document.Float("foo"));
            Assert.AreEqual(654.321f, document.Float("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_double_values()
        {
            var document = new Docunet()
                .Double("foo", 123.456)
                .Double("bar.baz", 654.321);
            
            Assert.AreEqual(123.456, document.Double("foo"));
            Assert.AreEqual(654.321, document.Double("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_decimal_values()
        {
            var document = new Docunet()
                .Decimal("foo", new Decimal(123.456))
                .Decimal("bar.baz", new Decimal(654.321));
            
            Assert.AreEqual(new Decimal(123.456), document.Decimal("foo"));
            Assert.AreEqual(new Decimal(654.321), document.Decimal("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_string_values()
        {
            var document = new Docunet()
                .String("foo", "test string value")
                .String("bar.baz", "test value string");
            
            Assert.AreEqual("test string value", document.String("foo"));
            Assert.AreEqual("test value string", document.String("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_dateTime_values()
        {
            var dateNow = DateTime.Now;
            var dateUtcNow = DateTime.UtcNow;
            
            var document = new Docunet()
                .DateTime("foo", dateNow)
                .DateTime("bar.baz", dateUtcNow);
            
            Assert.AreEqual(dateNow, document.DateTime("foo"));
            Assert.AreEqual(dateUtcNow, document.DateTime("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_object_values()
        {
            var document = new Docunet()
                .Object("foo", null)
                .Object("bar.baz", 123);
            
            Assert.AreEqual(null, document.Object("foo"));
            Assert.AreEqual(123, document.Object("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_generic_object_values()
        {
            var dummy1 = new Dummy();
            dummy1.Foo = "test string value";
            dummy1.Bar = 12345;
            
            var dummy2 = new Dummy();
            dummy2.Foo = "test value string";
            dummy2.Bar = 54321;
            
            var document = new Docunet()
                .Object("foo", dummy1)
                .Object("bar.baz", dummy2);
            
            Assert.AreEqual(dummy1, document.Object<Dummy>("foo"));
            Assert.AreEqual(dummy2, document.Object<Dummy>("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_document()
        {
            var doc = new Docunet()
                .String("foo", "test string value")
                .String("bar.baz", "test value string");
            
            var document = new Docunet()
                .Document("doc", doc);
            
            Assert.AreEqual(doc, document.Document("doc"));
        }
        
        [Test()]
        public void Should_get_and_set_dictionary()
        {
            var doc = new Dictionary<string, object>();
            doc.Add("foo", "test string value");
            doc.Add("bar.baz", "test value string");
            
            var document = new Docunet()
                .Document("doc", doc);
            
            var parsedDoc = Docunet.ToDocument(doc);
            
            Assert.AreEqual(parsedDoc, document.Document("doc"));
        }
        
        [Test()]
        public void Should_get_and_set_flat_list()
        {
            var stringList = new List<string> { "one", "two", "three" };
            var intList = new List<int> { 1, 2, 3 };
            
            var document = new Docunet()
                .List("foo", stringList)
                .List("bar.baz", intList);
            
            Assert.AreEqual(stringList, document.List<string>("foo"));
            Assert.AreEqual(intList, document.List<int>("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_generic_object_list()
        {
            var dummies1 = new List<Dummy> 
            { 
                new Dummy() { Foo = "Dummy one", Bar = 1 },
                new Dummy() { Foo = "Dummy two", Bar = 2 },
                new Dummy() { Foo = "Dummy three", Bar = 3 }
            };
            
            var dummies2 = new List<Dummy> 
            { 
                new Dummy() { Foo = "Dummy four", Bar = 4 },
                new Dummy() { Foo = "Dummy five", Bar = 5 },
                new Dummy() { Foo = "Dummy six", Bar = 6 }
            };
            
            var document = new Docunet()
                .List<Dummy>("foo", dummies1)
                .List<Dummy>("bar.baz", dummies2);
            
            Assert.AreEqual(dummies1, document.List<Dummy>("foo"));
            Assert.AreEqual(dummies2, document.List<Dummy>("bar.baz"));
        }
    }
}