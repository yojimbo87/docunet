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
            var document1 = new Document()
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
            var document1 = new Document()
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
            var document1 = new Document()
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
        
        [Test()]
        public void Should_evaluate_equality_of_two_documents()
        {
            var list = new List<string> { "one", "two", "three" };
            
            var dummies = new List<Document> 
            { 
                new Document().String("foo", "Dummy one").Int("bar", 1),
                new Document().String("foo", "Dummy two").Int("bar", 2),
                new Document().String("foo", "Dummy three").Int("bar", 3)
            };
            
            var document1 = new Document()
                .String("foo", "test string value")
                .String("bar.baz1", "test value string")
                .List("bar.baz2", list)
                .List("bar.baz3", dummies);
            
            var document2 = new Document()
                .String("foo", "test string value")
                .String("bar.baz1", "test value string")
                .List("bar.baz2", list)
                .List("bar.baz3", dummies);
            
            var document3 = new Document()
                .String("foo", "test string value");
            
            var document4 = new Document()
                .String("foo", "test string value")
                .String("bar.baz1", "test value string")
                .List("bar.baz2", new List<string> { "one", "two" })
                .List("bar.baz3", dummies);
            
            Assert.AreEqual(true, document1.Equals(document2));
            Assert.AreEqual(false, document1.Equals(document3));
            Assert.AreEqual(false, document1.Equals(document4));
        }
        
        [Test()]
        public void Should_convert_from_docunet_to_generic_object()
        {
            var document1 = new Document()
                .String("Foo", "foo string value")
                .Int("Bar", 12345);
            
            var dummy1 = document1.ToObject<Dummy>();
            
            Assert.AreEqual(document1.String("Foo"), dummy1.Foo);
            Assert.AreEqual(document1.Int("Bar"), dummy1.Bar);
            
            var stringList = new List<string> { "one", "two", "three" };
            var objectList = new List<Dummy>
            {
                new Dummy() { Foo = "one", Bar = 1 },
                new Dummy() { Foo = "two", Bar = 2 },
                new Dummy() { Foo = "three", Bar = 3 }
            };
            
            var document2 = new Document()
                .String("Foo", "foo string value")
                .Int("Bar", 12345)
                .Object("Baz", dummy1)
                .List<string>("StringList", stringList)
                .List<Dummy>("ObjectList", objectList);
                
            var nestedDummy = document2.ToObject<NestedDummy>();
            
            Assert.AreEqual(document2.String("Foo"), nestedDummy.Foo);
            Assert.AreEqual(document2.Int("Bar"), nestedDummy.Bar);
            Assert.AreEqual(document2.Object("Baz"), nestedDummy.Baz);
            Assert.AreEqual(document2.List<string>("StringList"), nestedDummy.StringList);
            Assert.AreEqual(document2.List<Dummy>("ObjectList"), nestedDummy.ObjectList);
        }
        
        [Test()]
        public void Should_replace_fields_with_another_document()
        {
            var sourceDocument = new Document()
                .String("foo1", "test string value one")
                .Int("bar.baz1", 12345);
            
            var document = new Document()
                .String("foo2", "test string value two")
                .Int("bar.baz2", 54321);
            
            document.Replace(sourceDocument);
            
            Assert.AreEqual(sourceDocument.String("foo1"), document.String("foo1"));
            Assert.AreEqual(false, document.Has("foo2"));
            Assert.AreEqual(sourceDocument.Int("bar.baz1"), document.Int("bar.baz1"));
            Assert.AreEqual(false, document.Has("bar.baz2"));
        }
        
        [Test()]
        public void Should_merge_two_documents_with_static_method()
        {
            var document1 = new Document()
                .String("foo1", "test string value one")
                .Int("bar.baz1", 12345)
                .List("intList", new List<int> { 1, 2, 3 })
                .List("nest.stringList", new List<string> { "a", "b", "c" });
            
            var document2 = new Document()
                .String("foo2", "test string value two")
                .Int("bar.baz2", 54321)
                .List("intList", new List<int> { 3, 4, 5 })
                .List("nest.stringList", new List<string> { "c", "d", "e" });
            
            var mergedDocument = Document.Merge(document1, document2);
            
            // check if document1 and document2 consists only of original values
            Assert.AreEqual(true, document1.Has("foo1"));
            Assert.AreEqual(true, document1.Has("bar.baz1"));
            Assert.AreEqual(true, document1.Has("intList"));
            Assert.AreEqual(true, document1.Has("nest.stringList"));
            Assert.AreEqual(false, document1.Has("foo2"));
            Assert.AreEqual(false, document1.Has("bar.baz2"));
            Assert.AreEqual(false, document2.Has("foo1"));
            Assert.AreEqual(false, document2.Has("bar.baz1"));
            Assert.AreEqual(true, document2.Has("foo2"));
            Assert.AreEqual(true, document2.Has("bar.baz2"));
            Assert.AreEqual(true, document2.Has("intList"));
            Assert.AreEqual(true, document2.Has("nest.stringList"));
            
            // check if merged document consists of combination of document1 and document2
            Assert.AreEqual(document1.String("foo1"), mergedDocument.String("foo1"));
            Assert.AreEqual(document2.String("foo2"), mergedDocument.String("foo2"));
            Assert.AreEqual(document1.Int("bar.baz1"), mergedDocument.Int("bar.baz1"));
            Assert.AreEqual(document2.Int("bar.baz2"), mergedDocument.Int("bar.baz2"));
            Assert.AreEqual(new List<int> { 1, 2, 3, 4, 5 }, mergedDocument.List<int>("intList"));
            Assert.AreEqual(new List<string> { "a", "b", "c", "d", "e" }, mergedDocument.List<string>("nest.stringList"));
        }
        
        [Test()]
        public void Should_merge_two_documents_by_replacing_existing_fields_with_second_document_with_static_method()
        {
            var document1 = new Document()
                .String("foo1", "test string value one")
                .Int("bar.baz1", 12345)
                .List("intList", new List<int> { 1, 2, 3 })
                .List("nest.stringList", new List<string> { "a", "b", "c" });
            
            var document2 = new Document()
                .String("foo2", "test string value two")
                .Int("bar.baz2", 54321)
                .List("intList", new List<int> { 3, 4, 5 })
                .List("nest.stringList", new List<string> { "c", "d", "e" });
            
            var mergedDocument = Document.Merge(document1, document2, MergeOptions.ReplaceFields);
            
            // check if document1 and document2 consists only of original values
            Assert.AreEqual(true, document1.Has("foo1"));
            Assert.AreEqual(true, document1.Has("bar.baz1"));
            Assert.AreEqual(true, document1.Has("intList"));
            Assert.AreEqual(true, document1.Has("nest.stringList"));
            Assert.AreEqual(false, document1.Has("foo2"));
            Assert.AreEqual(false, document1.Has("bar.baz2"));
            Assert.AreEqual(false, document2.Has("foo1"));
            Assert.AreEqual(false, document2.Has("bar.baz1"));
            Assert.AreEqual(true, document2.Has("foo2"));
            Assert.AreEqual(true, document2.Has("bar.baz2"));
            Assert.AreEqual(true, document2.Has("intList"));
            Assert.AreEqual(true, document2.Has("nest.stringList"));
            
            // check if merged document consists of combination of document1 and document2
            Assert.AreEqual(document1.String("foo1"), mergedDocument.String("foo1"));
            Assert.AreEqual(document2.String("foo2"), mergedDocument.String("foo2"));
            Assert.AreEqual(false, mergedDocument.Has("bar.baz1"));
            Assert.AreEqual(document2.Int("bar.baz2"), mergedDocument.Int("bar.baz2"));
            Assert.AreEqual(new List<int> { 3, 4, 5 }, mergedDocument.List<int>("intList"));
            Assert.AreEqual(new List<string> { "c", "d", "e" }, mergedDocument.List<string>("nest.stringList"));
        }
        
        [Test()]
        public void Should_merge_two_documents_by_keeping_existing_fields_from_first_document_with_static_method()
        {
            var document1 = new Document()
                .String("foo1", "test string value one")
                .Int("bar.baz1", 12345)
                .List("intList", new List<int> { 1, 2, 3 })
                .List("nest.stringList", new List<string> { "a", "b", "c" });
            
            var document2 = new Document()
                .String("foo2", "test string value two")
                .Int("bar.baz2", 54321)
                .List("intList", new List<int> { 3, 4, 5 })
                .List("nest.stringList", new List<string> { "c", "d", "e" });
            
            var mergedDocument = Document.Merge(document1, document2, MergeOptions.KeepFields);
            
            // check if document1 and document2 consists only of original values
            Assert.AreEqual(true, document1.Has("foo1"));
            Assert.AreEqual(true, document1.Has("bar.baz1"));
            Assert.AreEqual(true, document1.Has("intList"));
            Assert.AreEqual(true, document1.Has("nest.stringList"));
            Assert.AreEqual(false, document1.Has("foo2"));
            Assert.AreEqual(false, document1.Has("bar.baz2"));
            Assert.AreEqual(false, document2.Has("foo1"));
            Assert.AreEqual(false, document2.Has("bar.baz1"));
            Assert.AreEqual(true, document2.Has("foo2"));
            Assert.AreEqual(true, document2.Has("bar.baz2"));
            Assert.AreEqual(true, document2.Has("intList"));
            Assert.AreEqual(true, document2.Has("nest.stringList"));
            
            // check if merged document consists of combination of document1 and document2
            Assert.AreEqual(document1.String("foo1"), mergedDocument.String("foo1"));
            Assert.AreEqual(document2.String("foo2"), mergedDocument.String("foo2"));
            Assert.AreEqual(document1.Int("bar.baz1"), mergedDocument.Int("bar.baz1"));
            Assert.AreEqual(false, mergedDocument.Has("bar.baz2"));
            Assert.AreEqual(new List<int> { 1, 2, 3 }, mergedDocument.List<int>("intList"));
            Assert.AreEqual(new List<string> { "a", "b", "c" }, mergedDocument.List<string>("nest.stringList"));
        }
        
        [Test()]
        public void Should_merge_two_documents()
        {
            var document1 = new Document()
                .String("foo1", "test string value one")
                .Int("bar.baz1", 12345)
                .List("intList", new List<int> { 1, 2, 3 })
                .List("nest.stringList", new List<string> { "a", "b", "c" });
            
            var document2 = new Document()
                .String("foo2", "test string value two")
                .Int("bar.baz2", 54321)
                .List("intList", new List<int> { 3, 4, 5 })
                .List("nest.stringList", new List<string> { "c", "d", "e" });
            
            document1.Merge(document2);
            
            // check if document1 changed and document2 consists only of original values
            Assert.AreEqual(true, document1.Has("foo1"));
            Assert.AreEqual(true, document1.Has("bar.baz1"));
            Assert.AreEqual(true, document1.Has("intList"));
            Assert.AreEqual(true, document1.Has("nest.stringList"));
            Assert.AreEqual(true, document1.Has("foo2"));
            Assert.AreEqual(true, document1.Has("bar.baz2"));
            Assert.AreEqual(false, document2.Has("foo1"));
            Assert.AreEqual(false, document2.Has("bar.baz1"));
            Assert.AreEqual(true, document2.Has("foo2"));
            Assert.AreEqual(true, document2.Has("bar.baz2"));
            Assert.AreEqual(true, document2.Has("intList"));
            Assert.AreEqual(true, document2.Has("nest.stringList"));
            
            // check if merged document consists of combination of document1 and document2
            Assert.AreEqual("test string value one", document1.String("foo1"));
            Assert.AreEqual(document2.String("foo2"), document1.String("foo2"));
            Assert.AreEqual(12345, document1.Int("bar.baz1"));
            Assert.AreEqual(document2.Int("bar.baz2"), document1.Int("bar.baz2"));
            Assert.AreEqual(new List<string> { "a", "b", "c", "d", "e" }, document1.List<string>("nest.stringList"));
        }
        
        [Test()]
        public void Should_merge_two_documents_by_replacing_existing_fields_with_second_document()
        {
            var document1 = new Document()
                .String("foo1", "test string value one")
                .Int("bar.baz1", 12345)
                .List("intList", new List<int> { 1, 2, 3 })
                .List("nest.stringList", new List<string> { "a", "b", "c" });
            
            var document2 = new Document()
                .String("foo2", "test string value two")
                .Int("bar.baz2", 54321)
                .List("intList", new List<int> { 3, 4, 5 })
                .List("nest.stringList", new List<string> { "c", "d", "e" });
            
            document1.Merge(document2, MergeOptions.ReplaceFields);
            
            // check if document1 changed and document2 consists only of original values
            Assert.AreEqual(true, document1.Has("foo1"));
            Assert.AreEqual(false, document1.Has("bar.baz1"));
            Assert.AreEqual(true, document1.Has("intList"));
            Assert.AreEqual(true, document1.Has("nest.stringList"));
            Assert.AreEqual(true, document1.Has("foo2"));
            Assert.AreEqual(true, document1.Has("bar.baz2"));
            Assert.AreEqual(false, document2.Has("foo1"));
            Assert.AreEqual(false, document2.Has("bar.baz1"));
            Assert.AreEqual(true, document2.Has("foo2"));
            Assert.AreEqual(true, document2.Has("bar.baz2"));
            Assert.AreEqual(true, document2.Has("intList"));
            Assert.AreEqual(true, document2.Has("nest.stringList"));
            
            // check if merged document consists of combination of document1 and document2
            Assert.AreEqual("test string value one", document1.String("foo1"));
            Assert.AreEqual(document2.String("foo2"), document1.String("foo2"));
            Assert.AreEqual(false, document1.Has("bar.baz1"));
            Assert.AreEqual(document2.Int("bar.baz2"), document1.Int("bar.baz2"));
            Assert.AreEqual(new List<int> { 3, 4, 5 }, document1.List<int>("intList"));
            Assert.AreEqual(new List<string> { "c", "d", "e" }, document1.List<string>("nest.stringList"));
        }
        
        [Test()]
        public void Should_merge_two_documents_by_keeping_existing_fields_from_first_document()
        {
            var document1 = new Document()
                .String("foo1", "test string value one")
                .Int("bar.baz1", 12345)
                .List("intList", new List<int> { 1, 2, 3 })
                .List("nest.stringList", new List<string> { "a", "b", "c" });
            
            var document2 = new Document()
                .String("foo2", "test string value two")
                .Int("bar.baz2", 54321)
                .List("intList", new List<int> { 3, 4, 5 })
                .List("nest.stringList", new List<string> { "c", "d", "e" });
            
            document1.Merge(document2, MergeOptions.KeepFields);
            
            // check if document1 changed and document2 consists only of original values
            Assert.AreEqual(true, document1.Has("foo1"));
            Assert.AreEqual(true, document1.Has("bar.baz1"));
            Assert.AreEqual(true, document1.Has("intList"));
            Assert.AreEqual(true, document1.Has("nest.stringList"));
            Assert.AreEqual(true, document1.Has("foo2"));
            Assert.AreEqual(false, document1.Has("bar.baz2"));
            Assert.AreEqual(false, document2.Has("foo1"));
            Assert.AreEqual(false, document2.Has("bar.baz1"));
            Assert.AreEqual(true, document2.Has("foo2"));
            Assert.AreEqual(true, document2.Has("bar.baz2"));
            Assert.AreEqual(true, document2.Has("intList"));
            Assert.AreEqual(true, document2.Has("nest.stringList"));
            
            // check if merged document consists of combination of document1 and document2
            Assert.AreEqual("test string value one", document1.String("foo1"));
            Assert.AreEqual(document2.String("foo2"), document1.String("foo2"));
            Assert.AreEqual(12345, document1.Int("bar.baz1"));
            Assert.AreEqual(false, document1.Has("bar.baz2"));
            Assert.AreEqual(new List<int> { 1, 2, 3 }, document1.List<int>("intList"));
            Assert.AreEqual(new List<string> { "a", "b", "c" }, document1.List<string>("nest.stringList"));
        }
    }
}
