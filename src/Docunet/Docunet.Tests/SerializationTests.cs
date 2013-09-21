using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Docunet;

namespace Docunet.Tests
{
    [TestFixture()]
    public class SerializationTests
    {       
        [Test()]
        public void Should_serialize_null()
        {
            // fill document with data
            var document = new Document()
                .Object("null", null)
                .Object("embedded.null", null);

            // check if document data types are equal on retrieval
            Assert.AreEqual(null, document.Object("null"));
            Assert.AreEqual(null, document.Object("embedded.null"));
            
            // compare json representation of document
            var expected = "{\"null\":null,\"embedded\":{\"null\":null}}";
            var actual = document.Serialize();

            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void Should_serialize_boolean()
        {
            // fill document with data
            var document = new Document()
                .Bool("isTrue", true)
                .Bool("isFalse", false)
                .Bool("embedded.isTrue", true)
                .Bool("embedded.isFalse", false)
                .List("array", new List<bool> { true, false });

            // check if document data types are equal on retrieval
            Assert.AreEqual(true, document.Bool("isTrue"));
            Assert.AreEqual(false, document.Bool("isFalse"));
            Assert.AreEqual(true, document.Bool("embedded.isTrue"));
            Assert.AreEqual(false, document.Bool("embedded.isFalse"));
            Assert.AreEqual(new List<bool> { true, false }, document.List<bool>("array"));
            
            // compare json representation of document
            var expected = "{\"isTrue\":true,\"isFalse\":false,\"embedded\":{\"isTrue\":true,\"isFalse\":false},\"array\":[true,false]}";
            var actual = document.Serialize();

            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void Should_serialize_numbers()
        {
            // fill document with data
            var document = new Document()
                .Int("integer", int.Parse("123"))
                .Float("float", float.Parse("3.14", CultureInfo.InvariantCulture))
                .Int("embedded.integer", int.Parse("123"))
                .Float("embedded.float", float.Parse("3.14", CultureInfo.InvariantCulture))
                .List("intArray", new List<int> {123, 456})
                .List("floatArray", new List<float> {2.34f, 4.567f});

            // check if document data types are equal on retrieval
            Assert.AreEqual(123, document.Int("integer"));
            Assert.AreEqual(3.14f, document.Float("float"));
            Assert.AreEqual(123, document.Int("embedded.integer"));
            Assert.AreEqual(3.14f, document.Float("embedded.float"));
            Assert.AreEqual(new List<int> {123, 456}, document.List<int>("intArray"));
            Assert.AreEqual(new List<float> {2.34f, 4.567f}, document.List<float>("floatArray"));
            
            // compare json representation of document
            var expected = "{\"integer\":123,\"float\":3.14,\"embedded\":{\"integer\":123,\"float\":3.14},\"intArray\":[123,456],\"floatArray\":[2.34,4.567]}";
            var actual = document.Serialize();

            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void Should_serialize_strings()
        {
            // fill document with data
            var document = new Document()
                .String("string", "foo bar")
                .String("embedded.string", "foo bar")
                .List("embedded.array", new List<string> { "foo", "bar" })
                .List("array", new List<string> { "foo", "bar" });

            // check if document data types are equal on retrieval
            Assert.AreEqual("foo bar", document.String("string"));
            Assert.AreEqual("foo bar", document.String("embedded.string"));
            Assert.AreEqual(new List<string> { "foo", "bar" }, document.List<string>("embedded.array"));
            Assert.AreEqual(new List<string> { "foo", "bar" }, document.List<string>("array"));

            // compare json representation of document
            var expected = "{\"string\":\"foo bar\",\"embedded\":{\"string\":\"foo bar\",\"array\":[\"foo\",\"bar\"]},\"array\":[\"foo\",\"bar\"]}";
            var actual = document.Serialize();

            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void Should_serialize_datetime()
        {
            var dateTimeIso = DateTime.Parse("2008-12-20T02:12:02.363Z").ToUniversalTime();
            var dateTimeUnix = DateTime.Parse("2008-12-20T02:12:02Z").ToUniversalTime();
            
            // fill document with data
            var document = new Document()
                .DateTime("datetime1", dateTimeIso, DateTimeFormat.Iso8601String)
                .DateTime("datetime2", dateTimeUnix, DateTimeFormat.UnixTimeStamp);

            // check if document data types are equal on retrieval
            Assert.AreEqual("2008-12-20T02:12:02.363Z", document.String("datetime1"));
            Assert.AreEqual(dateTimeIso, document.DateTime("datetime1"));
            
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = (dateTimeUnix - unixEpoch);
            
            Assert.AreEqual((long)span.TotalSeconds, document.Long("datetime2"));
            Assert.AreEqual(dateTimeUnix, document.DateTime("datetime2"));
            
            // compare json representation of document
            var expected = "{\"datetime1\":\"2008-12-20T02:12:02.363Z\",\"datetime2\":" + (long)span.TotalSeconds + "}";
            var actual = document.Serialize();

            Assert.AreEqual(expected, actual);
        }
    }
}
