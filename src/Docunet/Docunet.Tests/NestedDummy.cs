using System.Collections.Generic;

namespace Docunet.Tests
{
    public class NestedDummy
    {
        public string Foo { get; set; }
        public int Bar { get; set; }
        public Dummy Baz { get; set; }
        public List<string> StringList { get; set; }
        public List<Dummy> ObjectList { get; set; }
    }
}
