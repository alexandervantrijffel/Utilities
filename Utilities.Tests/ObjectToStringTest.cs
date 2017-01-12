using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace Structura.Shared.Utilities.Tests
{
    public class ContentsAsStringTest
    {
        class ItemWithCollection
        {
            public IList<Guid> Guids { get; set; }
        }
        
        [Fact]
        public void Generates_collection_item_details()
        {
            var dto = new ItemWithCollection { Guids = new List<Guid> { Guid.NewGuid() } };
            new ContentsAsString(dto).ToString().ShouldContain(dto.Guids.First().ToString());
        }

        [Fact]
        public void Dumps_string_without_meta_data()
        {
            var s = "This is allright.";
            new ContentsAsString(s).ToString().ShouldBe(s);
        }

        [Fact]
        public void Dumps_collection_of_collections()
        {
            var data = new Dictionary<string, string[]>();
            data.Add("the key", new[] { "value1", "value2" });
            string result = new ContentsAsString(data);
            result.ShouldContain("the key");
            result.ShouldContain("value1");
            result.ShouldContain("value2");
        }

        [Fact]
        public void Dumps_rich_property_field()
        {
            var data = new AClass {Inner = new AClass {AString = "it's me"}};
            string result = new ContentsAsString(data);
            result.ShouldContain("it's me");
        }

        [Fact]
        public void Dumps_enum_text()
        {
            var data = new AClass {AnEnum = AnEnum.FirstItem};
            string result = new ContentsAsString(data);
            result.ShouldContain("FirstItem");
        }
    }

    public enum AnEnum
    {
        FirstItem
    }
    public class AClass
    {
        public AClass Inner { get; set; }
        public AnEnum AnEnum { get; set; }
        public string AString { get; set; }
    }
}