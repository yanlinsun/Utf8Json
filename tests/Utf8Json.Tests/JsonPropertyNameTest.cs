using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Utf8Json.Tests
{

#if !NET45


    public class JsonPropertyNameTest 
    {
        [DataContract]
        public class MyClass
        {
            [DataMember]
            [JsonIgnore]
            public int MyProperty1 { get; set; }
            [DataMember(Name = "mp2")]
            public string MyProperty2;
        }

        [DataContract]
        public class MyClass1
        {
            [DataMember(Name = "mp1")]
            [JsonPropertyName("Mp1")]
            public int MyProperty1 { get; set; }
            [JsonPropertyName("Mp2")]
            [DataMember(Name = "mp2")]
            public string MyProperty2;
        }

        public class MyClass2
        {
            [JsonPropertyName("mp1")]
            public int MyProperty1 { get; set; }
            public string MyProperty2;
        }

        [Fact]
        public void JsonIgnoreHasHigherPriority()
        {
            var mc = new MyClass { MyProperty1 = 100, MyProperty2 = "foobar" };
            var bin = JsonSerializer.Serialize(mc);

            Encoding.UTF8.GetString(bin).Is(@"{""mp2"":""foobar""}");
            var mc2 = JsonSerializer.Deserialize<MyClass>(bin);

            mc2.MyProperty1.Is(0);
            mc.MyProperty2.Is(mc2.MyProperty2);

            bin = Encoding.UTF8.GetBytes(@"{""mp1"":100,""mp2"":""foobar""}");

            mc2 = JsonSerializer.Deserialize<MyClass>(bin);
            mc2.MyProperty1.Is(0);
            mc.MyProperty2.Is(mc2.MyProperty2);
        }

        [Fact]
        public void JsonPropertyNameHasHigherPriority()
        {
            var mc = new MyClass1 { MyProperty1 = 100, MyProperty2 = "foobar" };

            var bin = JsonSerializer.Serialize(mc);

            Encoding.UTF8.GetString(bin).Is(@"{""Mp1"":100,""Mp2"":""foobar""}");

            var mc2 = JsonSerializer.Deserialize<MyClass1>(bin);

            mc.MyProperty1.Is(mc2.MyProperty1);
            mc.MyProperty2.Is(mc2.MyProperty2);
        }

        [Fact]
        public void Serialize()
        {
            var mc = new MyClass2 { MyProperty1 = 100, MyProperty2 = "foobar" };

            var bin = JsonSerializer.Serialize(mc);
            var mc2 = JsonSerializer.Deserialize<MyClass2>(bin);

            mc.MyProperty1.Is(mc2.MyProperty1);
            mc.MyProperty2.Is(mc2.MyProperty2);

            Encoding.UTF8.GetString(bin).Is(@"{""mp1"":100,""MyProperty2"":""foobar""}");
        }
    }
#endif

}
