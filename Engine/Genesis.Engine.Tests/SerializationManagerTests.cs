using Xunit;
using Genesis.Engine.Core.Runtime.Serialization;
using System.Collections.Generic;

public class SerializationManagerTests
{
    private class TestRecord { public int Id { get; set; } public string Name { get; set; } }

    [Fact]
    public void FallbackSerializesPocoAndList()
    {
        var mgr = new SerializationManager();
        var data = new List<TestRecord> { new TestRecord{ Id=1, Name="A"}, new TestRecord{ Id=2, Name="B"} };
        var json = mgr.Serialize(data);
        var loaded = mgr.Deserialize<List<TestRecord>>(json);
        Assert.Equal(2, loaded.Count);
        Assert.Equal("A", loaded[0].Name);
    }

    [Fact]
    public void CustomSerializerTakesPrecedence()
    {
        var mgr = new SerializationManager();
        // register a trivial custom serializer for TestRecord
        mgr.Register<TestRecord>(new SimpleTestRecordSerializer());
        var rec = new TestRecord { Id = 42, Name = "X" };
        var json = mgr.Serialize(rec);
        // SimpleTestRecordSerializer serializes as "CUSTOM"
        Assert.Equal("\"CUSTOM\"", json);
        var back = mgr.Deserialize<TestRecord>(json);
        Assert.Equal(42, back.Id);
    }

    private class SimpleTestRecordSerializer : ISerializer<TestRecord>
    {
        public string Serialize(TestRecord obj) => "\"CUSTOM\"";
        public TestRecord Deserialize(string json) => new TestRecord { Id = 42, Name = "from-custom" };
    }
}
