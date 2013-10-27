using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Docunet;

namespace Docunet.Tests
{
    [TestFixture()]
    public class DeserializationTests
    {
        [Test()]
        public void Should_deserialize_null()
        {
            var json = "{\"null\":null,\"embedded\":{\"null\":null}}";
            var document = new Document(json);

            // check for fields existence
            Assert.AreEqual(true, document.Has("null"));
            Assert.AreEqual(true, document.Has("embedded"));
            Assert.AreEqual(true, document.Has("embedded.null"));
            
            // check if the field has null values
            Assert.AreEqual(true, document.IsNull("null"));
            Assert.AreEqual(false, document.IsNull("embedded"));
            Assert.AreEqual(true, document.IsNull("embedded.null"));

            // check for fields values
            Assert.AreEqual(null, document.Object("null"));
            Assert.AreEqual(null, document.Object("embedded.null"));
            
            // check generated json value
            Assert.AreEqual(json, document.Serialize());
        }
        
        [Test()]
        public void Should_deserialize_boolean()
        {
            var json = "{\"isTrue\":true,\"isFalse\":false,\"embedded\":{\"isTrue\":true,\"isFalse\":false},\"array\":[true,false]}";
            var document = new Document(json);

            // check for fields existence
            Assert.AreEqual(true, document.Has("isTrue"));
            Assert.AreEqual(true, document.Has("isFalse"));
            Assert.AreEqual(true, document.Has("embedded"));
            Assert.AreEqual(true, document.Has("embedded.isTrue"));
            Assert.AreEqual(true, document.Has("embedded.isFalse"));
            Assert.AreEqual(true, document.Has("array"));
            
            // check for fields values
            Assert.AreEqual(true, document.Bool("isTrue"));
            Assert.AreEqual(false, document.Bool("isFalse"));
            Assert.AreEqual(true, document.Bool("embedded.isTrue"));
            Assert.AreEqual(false, document.Bool("embedded.isFalse"));            
            Assert.AreEqual(new List<bool> { true, false }, document.List<bool>("array"));
            
            // check generated json value
            Assert.AreEqual(json, document.Serialize());
        }
        
        [Test()]
        public void Should_deserialize_numbers()
        {
            var json = "{\"integer\":123,\"float\":3.14,\"embedded\":{\"integer\":123,\"float\":3.14},\"intArray\":[123,456],\"floatArray\":[2.34,4.567]}";
            var document = new Document(json);
            
            // check for fields existence
            Assert.AreEqual(true, document.Has("integer"));
            Assert.AreEqual(true, document.Has("float"));
            Assert.AreEqual(true, document.Has("embedded"));
            Assert.AreEqual(true, document.Has("embedded.integer"));
            Assert.AreEqual(true, document.Has("embedded.float"));
            Assert.AreEqual(true, document.Has("intArray"));
            Assert.AreEqual(true, document.Has("floatArray"));
            
            // check for fields values
            Assert.AreEqual((int)123, document.Int("integer"));
            Assert.AreEqual(3.14f, document.Float("float"));
            Assert.AreEqual(123, document.Int("embedded.integer"));
            Assert.AreEqual(3.14f, document.Float("embedded.float"));
            Assert.AreEqual(new List<int> { 123, 456 }, document.List<int>("intArray"));
            Assert.AreEqual(new List<float> { 2.34f, 4.567f }, document.List<float>("floatArray"));

            // check generated json value
            Assert.AreEqual(json, document.Serialize());
        }
        
        [Test()]
        public void Should_deserialize_strings()
        {
            var json = "{\"string\":\"foo bar\",\"embedded\":{\"string\":\"foo bar\",\"array\":[\"foo\",\"bar\"]},\"array\":[\"foo\",\"bar\"]}";
            var document = new Document(json);
            
            // check for fields existence
            Assert.AreEqual(true, document.Has("string"));
            Assert.AreEqual(true, document.Has("embedded"));
            Assert.AreEqual(true, document.Has("embedded.string"));
            Assert.AreEqual(true, document.Has("embedded.array"));
            Assert.AreEqual(true, document.Has("array"));
            
            // check for fields values
            Assert.AreEqual("foo bar", document.String("string"));
            Assert.AreEqual("foo bar", document.String("embedded.string"));
            Assert.AreEqual(new List<string> { "foo", "bar" }, document.List<string>("embedded.array"));
            Assert.AreEqual(new List<string> { "foo", "bar" }, document.List<string>("array"));
            
            // check generated json value
            Assert.AreEqual(json, document.Serialize());
        }
        
        [Test()]
        public void Should_deserialize_datetime()
        {
            var dateTimeIso = DateTime.Parse("2008-12-20T02:12:02.363Z").ToUniversalTime();
            var dateTimeUnix = DateTime.Parse("2008-12-20T02:12:02Z").ToUniversalTime();
            
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = (dateTimeUnix - unixEpoch);
            
            var json = "{\"datetime1\":\"2008-12-20T02:12:02.363Z\",\"datetime2\":" + (long)span.TotalSeconds + "}";
            var document = new Document(json);
            
            // check if the fields existence
            Assert.AreEqual(true, document.Has("datetime1"));
            Assert.AreEqual(true, document.Has("datetime2"));
            
            // check for fields values
            Assert.AreEqual(dateTimeIso, document.DateTime("datetime1"));
            Assert.AreEqual(dateTimeUnix, document.DateTime("datetime2"));
        }
        
        [Test()]
        public void Should_deserialize_arrays()
        {
            var intJson = "[1, 2, 3]";
            var stringJson = "[\"one\",\"two\",\"three\"]";
            var documentJson = "[{\"foo\":\"one\",\"bar\":1},{\"foo\":\"two\",\"bar\":2},{\"foo\":\"three\",\"bar\":3}]";
            var nestedDocumentJson = "[{\"foo\":\"one\",\"bar\":{\"foo\":[1,2,3],\"bar\":1}},{\"foo\":\"two\",\"bar\":{\"foo\":[1,2,3],\"bar\":2}},{\"foo\":\"three\",\"bar\":{\"foo\":[1,2,3],\"bar\":3}}]";
            
            var intArray = Document.DeserializeArray<int>(intJson);
            var stringArray = Document.DeserializeArray<string>(stringJson);
            var documentArray = Document.DeserializeArray<Document>(documentJson);
            var nestedDocumentArray = Document.DeserializeArray<Document>(nestedDocumentJson);
            
            Assert.AreEqual(new List<int> { 1, 2, 3 }, intArray);
            Assert.AreEqual(new List<string> { "one", "two", "three" }, stringArray);
            
            var expectedDocumentArray = new List<Document>
            {
                new Document().String("foo", "one").Int("bar", 1),
                new Document().String("foo", "two").Int("bar", 2),
                new Document().String("foo", "three").Int("bar", 3)
            };
            
            Assert.AreEqual(expectedDocumentArray, documentArray);
            
            var expectedNestedDocumentArray = new List<Document>
            {
                new Document()
                    .String("foo", "one")
                    .Object("bar", new Document()
                              .List<int>("foo", new List<int> { 1, 2, 3 })
                              .Int("bar", 1)
                    ),
                new Document()
                    .String("foo", "two")
                    .Object("bar", new Document()
                              .List<int>("foo", new List<int> { 1, 2, 3 })
                              .Int("bar", 2)
                    ),
                new Document()
                    .String("foo", "three")
                    .Object("bar", new Document()
                              .List<int>("foo", new List<int> { 1, 2, 3 })
                              .Int("bar", 3)
                    )
            };
            
            Assert.AreEqual(expectedNestedDocumentArray, nestedDocumentArray);
        }
    }
}
