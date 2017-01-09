using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace Structura.Shared.Utilities.Tests
{
    public class ObjectToStringTest
    {
        class ItemWithCollection
        {
            public IList<Guid> Guids { get; set; }
        }

        [Fact]
        public void GeneratesCollectionItemDetails()
        {
            var dto = new ItemWithCollection { Guids = new List<Guid> { Guid.NewGuid() } };
            ObjectToString.DumpTypeAndFields(dto).ShouldContain(dto.Guids.First().ToString());
        }

        [Fact]
        public void Dumps_string_without_meta_data()
        {
            var s = "This is allright.";
            ObjectToString.DumpTypeAndFields(s).ShouldBe(s);
        }
    }
}