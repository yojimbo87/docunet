﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Should_get_and_set_dateTime_values_with_local_settings()
        {
            var dateNow = DateTime.Now;
            var dateUtcNow = DateTime.UtcNow;
            
            var document = new Docunet()
                .DateTime("foo", dateNow)
                .DateTime("bar.baz", dateUtcNow)
                .DateTime("baz1", dateUtcNow, DateTimeFormat.Iso8601String)
                .DateTime("baz2", dateUtcNow, DateTimeFormat.UnixTimeStamp);
            
            Assert.AreEqual(dateNow, document.DateTime("foo"));
            Assert.AreEqual(dateUtcNow, document.DateTime("bar.baz"));
            Assert.AreEqual(dateUtcNow.ToString(), document.DateTime("baz1").ToString());
            Assert.AreEqual(dateUtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK"), document.String("baz1"));
            Assert.AreEqual(dateUtcNow.ToString("yyyy-MM-dd HH:mm:ss"), document.DateTime("baz2").ToString("yyyy-MM-dd HH:mm:ss"));
            
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = (dateUtcNow.ToUniversalTime() - unixEpoch);
            Assert.AreEqual((long)span.TotalSeconds, document.Long("baz2"));
        }
        
        [Test()]
        public void Should_get_and_set_dateTime_values_with_global_settings()
        {
            var dateNow = DateTime.Now;
            var dateUtcNow = DateTime.UtcNow;
            
            var document = new Docunet();
            
            Docunet.Settings.DateTimeFormat = DateTimeFormat.Iso8601String;
            document.DateTime("baz1", dateUtcNow);
            
            Docunet.Settings.DateTimeFormat = DateTimeFormat.UnixTimeStamp;
            document.DateTime("baz2", dateUtcNow);
            
            Docunet.Settings.DateTimeFormat = DateTimeFormat.DateTime;
            document.DateTime("baz3", dateUtcNow);
            
            Assert.AreEqual(dateUtcNow.ToString(), document.DateTime("baz1").ToString());
            Assert.AreEqual(dateUtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK"), document.String("baz1"));
            Assert.AreEqual(dateUtcNow.ToString("yyyy-MM-dd HH:mm:ss"), document.DateTime("baz2").ToString("yyyy-MM-dd HH:mm:ss"));
            
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = (dateUtcNow.ToUniversalTime() - unixEpoch);
            Assert.AreEqual((long)span.TotalSeconds, document.Long("baz2"));
            
            Assert.AreEqual(dateUtcNow, document.DateTime("baz3"));
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
            
            var dummy2 = new NestedDummy();
            dummy2.Foo = "test value string";
            dummy2.Bar = 54321;
            dummy2.Baz = dummy1;
            dummy2.StringList = new List<string> { "one", "two", "three" };
            dummy2.ObjectList = new List<Dummy> 
            {
                new Dummy() { Foo = "one", Bar = 1 },
                new Dummy() { Foo = "two", Bar = 2 },
                new Dummy() { Foo = "three", Bar = 3 }
            };
            
            var document = new Docunet()
                .Object("foo", dummy1)
                .Object("bar.baz", dummy2);
            
            Assert.AreEqual(dummy1, document.Object<Dummy>("foo"));
            Assert.AreEqual(dummy2, document.Object<NestedDummy>("bar.baz"));
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
        public void Should_check_for_null_value()
        {
            var document = new Docunet()
                .Object("foo1", null)
                .List("foo2", new List<object> { 1, 2, null })
                .String("foo3", "test string value")
                .Object("bar.baz1", null)
                .List("bar.baz2", new List<object> { 1, 2, null })
                .String("bar.baz3", "test string value");
            
            Assert.AreEqual(true, document.IsNull("foo1"));
            Assert.AreEqual(false, document.IsNull("foo2[1]"));
            Assert.AreEqual(true, document.IsNull("foo2[2]"));
            Assert.AreEqual(true, document.IsNull("bar.baz1"));
            Assert.AreEqual(false, document.IsNull("bar.baz2[1]"));
            Assert.AreEqual(true, document.IsNull("bar.baz2[2]"));
            Assert.AreEqual(false, document.IsNull("bar.baz3"));
        }
        
        [Test()]
        public void Should_get_and_set_generic_object_converted_to_document()
        {
            var dummy1 = new Dummy();
            dummy1.Foo = "test string value";
            dummy1.Bar = 12345;
            
            var dummy2 = new NestedDummy();
            dummy2.Foo = "test value string";
            dummy2.Bar = 54321;
            dummy2.Baz = dummy1;
            dummy2.StringList = new List<string> { "one", "two", "three" };
            dummy2.ObjectList = new List<Dummy> 
            {
                new Dummy() { Foo = "one", Bar = 1 },
                new Dummy() { Foo = "two", Bar = 2 },
                new Dummy() { Foo = "three", Bar = 3 }
            };
            
            var doc1 = Docunet.ToDocument(dummy1);
            var doc2 = Docunet.ToDocument(dummy2);
            
            var document = new Docunet()
                .Document("foo", dummy1)
                .Document("bar.baz", dummy2);
            
            Assert.AreEqual(doc1, document.Document("foo"));
            Assert.AreEqual(doc2, document.Document("bar.baz"));
            
            var stringList = document.List<string>("bar.baz.StringList");
            var objectList = document.List<Docunet>("bar.baz.ObjectList");
            
            Assert.AreEqual(dummy2.StringList, stringList);
            Assert.AreEqual(Docunet.ToList(dummy2.ObjectList), objectList);
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
                .List("foo", dummies1)
                .List("bar.baz", dummies2);
            
            Assert.AreEqual(dummies1, document.List<Dummy>("foo"));
            Assert.AreEqual(dummies2, document.List<Dummy>("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_docunet_list()
        {
            var dummies1 = new List<Docunet> 
            { 
                new Docunet().String("foo", "Dummy one").Int("bar", 1),
                new Docunet().String("foo", "Dummy two").Int("bar", 2),
                new Docunet().String("foo", "Dummy three").Int("bar", 3)
            };
            
            var dummies2 = new List<Docunet> 
            { 
                new Docunet().String("foo", "Dummy four").Int("bar", 4),
                new Docunet().String("foo", "Dummy five").Int("bar", 5),
                new Docunet().String("foo", "Dummy six").Int("bar", 6)
            };
            
            var document = new Docunet()
                .List("foo", dummies1)
                .List("bar.baz", dummies2);
            
            Assert.AreEqual(dummies1, document.List<Docunet>("foo"));
            Assert.AreEqual(dummies2, document.List<Docunet>("bar.baz"));
        }
        
        [Test()]
        public void Should_get_and_set_field_with_flat_array_values()
        {   
            var list1 = new List<string> { "one", "two", "three" };
            var list2 = new List<string> { "four", "five", "six" };
            
            var document = new Docunet()
                .List<string>("foo", list1)
                .List<string>("bar.baz", list2);
            
            Assert.AreEqual(list1[0], document.String("foo[0]"));
            Assert.AreEqual(list1[1], document.String("foo[1]"));
            Assert.AreEqual(list1[2], document.String("foo[2]"));
            
            Assert.AreEqual(list2[0], document.String("bar.baz[0]"));
            Assert.AreEqual(list2[1], document.String("bar.baz[1]"));
            Assert.AreEqual(list2[2], document.String("bar.baz[2]"));
        }
        
        [Test()]
        public void Should_get_and_set_field_with_object_array_values()
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
                .List("foo", dummies1)
                .List("bar.baz", dummies2);
            
            Assert.AreEqual(dummies1, document.List<Dummy>("foo"));
            Assert.AreEqual(dummies1[0].Foo, document.Object<Dummy>("foo[0]").Foo);
            Assert.AreEqual(dummies1[0].Bar, document.Object<Dummy>("foo[0]").Bar);
            Assert.AreEqual(dummies1[1].Foo, document.Object<Dummy>("foo[1]").Foo);
            Assert.AreEqual(dummies1[1].Bar, document.Object<Dummy>("foo[1]").Bar);
            Assert.AreEqual(dummies1[2].Foo, document.Object<Dummy>("foo[2]").Foo);
            Assert.AreEqual(dummies1[2].Bar, document.Object<Dummy>("foo[2]").Bar);
            
            Assert.AreEqual(dummies2, document.List<Dummy>("bar.baz"));
            Assert.AreEqual(dummies2[0].Foo, document.Object<Dummy>("bar.baz[0]").Foo);
            Assert.AreEqual(dummies2[0].Bar, document.Object<Dummy>("bar.baz[0]").Bar);
            Assert.AreEqual(dummies2[1].Foo, document.Object<Dummy>("bar.baz[1]").Foo);
            Assert.AreEqual(dummies2[1].Bar, document.Object<Dummy>("bar.baz[1]").Bar);
            Assert.AreEqual(dummies2[2].Foo, document.Object<Dummy>("bar.baz[2]").Foo);
            Assert.AreEqual(dummies2[2].Bar, document.Object<Dummy>("bar.baz[2]").Bar);
        }
        
        [Test()]
        public void Should_get_and_set_field_with_docunet_array_values()
        {
            var dummies1 = new List<Docunet> 
            { 
                new Docunet().String("foo", "Dummy one").Int("bar", 1),
                new Docunet().String("foo", "Dummy two").Int("bar", 2),
                new Docunet().String("foo", "Dummy three").Int("bar", 3)
            };
            
            var dummies2 = new List<Docunet> 
            { 
                new Docunet().String("foo", "Dummy four").Int("bar", 4),
                new Docunet().String("foo", "Dummy five").Int("bar", 5),
                new Docunet().String("foo", "Dummy six").Int("bar", 6)
            };
            
            var document = new Docunet()
                .List("foo", dummies1)
                .List("bar.baz", dummies2);
            
            Assert.AreEqual(dummies1, document.List<Docunet>("foo"));
            Assert.AreEqual(dummies1[0].String("foo"), document.String("foo[0].foo"));
            Assert.AreEqual(dummies1[0].Int("bar"), document.Int("foo[0].bar"));
            Assert.AreEqual(dummies1[1].String("foo"), document.String("foo[1].foo"));
            Assert.AreEqual(dummies1[1].Int("bar"), document.Int("foo[1].bar"));
            Assert.AreEqual(dummies1[2].String("foo"), document.String("foo[2].foo"));
            Assert.AreEqual(dummies1[2].Int("bar"), document.Int("foo[2].bar"));
            
            Assert.AreEqual(dummies2, document.List<Docunet>("bar.baz"));
            Assert.AreEqual(dummies2[0].String("foo"), document.String("bar.baz[0].foo"));
            Assert.AreEqual(dummies2[0].Int("bar"), document.Int("bar.baz[0].bar"));
            Assert.AreEqual(dummies2[1].String("foo"), document.String("bar.baz[1].foo"));
            Assert.AreEqual(dummies2[1].Int("bar"), document.Int("bar.baz[1].bar"));
            Assert.AreEqual(dummies2[2].String("foo"), document.String("bar.baz[2].foo"));
            Assert.AreEqual(dummies2[2].Int("bar"), document.Int("bar.baz[2].bar"));
        }
        
        [Test()]
        public void Should_check_presence_of_document_fields()
        {
            var document = new Docunet()
                .String("foo1", "test string value")
                .List("foo2", new List<string> { "one", "two", "three" })
                .String("bar.baz1", "test value string")
                .List("bar.baz2", new List<string> { "one", "two", "three" });
            
            Assert.AreEqual(true, document.Has("foo1"));
            Assert.AreEqual(true, document.Has("foo2"));
            Assert.AreEqual(true, document.Has("foo2[0]"));
            Assert.AreEqual(true, document.Has("foo2[1]"));
            Assert.AreEqual(true, document.Has("foo2[2]"));
            Assert.AreEqual(false, document.Has("foo2[3]"));
            Assert.AreEqual(true, document.Has("bar.baz1"));
            Assert.AreEqual(true, document.Has("bar.baz2"));
            Assert.AreEqual(true, document.Has("bar.baz2[0]"));
            Assert.AreEqual(true, document.Has("bar.baz2[1]"));
            Assert.AreEqual(true, document.Has("bar.baz2[2]"));
            Assert.AreEqual(false, document.Has("bar.baz2[3]"));
            Assert.AreEqual(false, document.Has("should_not_exist"));
        }
        
        [Test()]
        public void Should_drop_fields()
        {
            var document = new Docunet()
                .String("foo1", "test string value 1")
                .String("foo2", "test string value 2")
                .String("bar.baz1", "test value string 1")
                .String("bar.baz2", "test value string 2");
            
            document
                .Drop("foo2")
                .Drop("bar.baz2");
                      
            Assert.AreEqual("test string value 1", document.String("foo1"));
            Assert.AreEqual(null, document.String("foo2"));
            Assert.AreEqual("test value string 1", document.String("bar.baz1"));
            Assert.AreEqual(null, document.String("bar.baz2"));
        }
        
        [Test()]
        public void Should_retrieve_type_of_fields()
        {
            var dummy = new Dummy();
            var stringList = new List<string>();           
            var objectList = new List<Dummy>();
            
            var document = new Docunet()
                .Bool("bool", true)
                .Byte("byte", 123)
                .Short("short", 12345)
                .Int("int", 1234567)
                .Long("long", 123456789)
                .Float("float", 123.4f)
                .Double("double", 123.456)
                .Decimal("decimal", new Decimal(12345))
                .String("string", "test string value")
                .DateTime("dateTime", DateTime.Now)
                .Object("dummyObject", dummy)
                .List("stringList", stringList)
                .List("objectList", objectList)
                .Bool("nested.bool", true)
                .Byte("nested.byte", 123)
                .Short("nested.short", 12345)
                .Int("nested.int", 1234567)
                .Long("nested.long", 123456789)
                .Float("nested.float", 123.4f)
                .Double("nested.double", 123.456)
                .Decimal("nested.decimal", new Decimal(12345))
                .String("nested.string", "test string value")
                .DateTime("nested.dateTime", DateTime.Now)
                .Object("nested.dummyObject", dummy)
                .List("nested.stringList", stringList)
                .List("nested.objectList", objectList);
            
            Assert.AreEqual(null, document.Type("shouldNotExist"));
            Assert.AreEqual(typeof(bool), document.Type("bool"));
            Assert.AreEqual(typeof(byte), document.Type("byte"));
            Assert.AreEqual(typeof(short), document.Type("short"));
            Assert.AreEqual(typeof(int), document.Type("int"));
            Assert.AreEqual(typeof(long), document.Type("long"));
            Assert.AreEqual(typeof(float), document.Type("float"));
            Assert.AreEqual(typeof(double), document.Type("double"));
            Assert.AreEqual(typeof(decimal), document.Type("decimal"));
            Assert.AreEqual(typeof(string), document.Type("string"));
            Assert.AreEqual(typeof(DateTime), document.Type("dateTime"));
            Assert.AreEqual(typeof(List<string>), document.Type("stringList"));
            Assert.AreEqual(typeof(List<Dummy>), document.Type("objectList"));
            Assert.AreEqual(typeof(bool), document.Type("nested.bool"));
            Assert.AreEqual(typeof(byte), document.Type("nested.byte"));
            Assert.AreEqual(typeof(short), document.Type("nested.short"));
            Assert.AreEqual(typeof(int), document.Type("nested.int"));
            Assert.AreEqual(typeof(long), document.Type("nested.long"));
            Assert.AreEqual(typeof(float), document.Type("nested.float"));
            Assert.AreEqual(typeof(double), document.Type("nested.double"));
            Assert.AreEqual(typeof(decimal), document.Type("nested.decimal"));
            Assert.AreEqual(typeof(string), document.Type("nested.string"));
            Assert.AreEqual(typeof(DateTime), document.Type("nested.dateTime"));
            Assert.AreEqual(typeof(List<string>), document.Type("nested.stringList"));
            Assert.AreEqual(typeof(List<Dummy>), document.Type("nested.objectList"));
        }
    }
}