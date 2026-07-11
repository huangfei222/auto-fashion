using Xunit;
using Genesis.Engine.Core.Runtime.Serialization;
using System.Collections.Generic;

public class SerializationManagerTests
{
    private class TestRecord
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    [Fact]
    public void FallbackSerializesPocoAndList()
    {
        var mgr = new SerializationManager();
        var data = new List<TestRecord>
        {
            new TestRecord { Id = 1, Name = "A" },
            new TestRecord { Id = 2, Name = "B" }
        };

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
        Assert.Equal("from-custom", back.Name);
    }

    [Fact]
    public void RegisterFromInstance_RegistersGenericSerializer()
    {
        var mgr = new SerializationManager();
        var ser = new TestRecordSerializer();

        // If SerializationManager.RegisterFromInstance exists, this should register the serializer.
        // If not yet implemented, this test will fail until the method is added.
        // The test is written to validate the intended behavior after the migration.
        mgr.RegisterFromInstance(ser);

        Assert.True(mgr.TryGetSerializer<TestRecord>(out var got));
        Assert.NotNull(got);

        var json = mgr.Serialize(new TestRecord { Id = 7, Name = "Z" });
        // TestRecordSerializer serializes to a JSON containing Id:7 and Name:"from-custom"
        Assert.Contains("\"Id\":7", json);
    }

    private class SimpleTestRecordSerializer : ISerializer<TestRecord>
    {
        public string Serialize(TestRecord obj) => "\"CUSTOM\"";
        public TestRecord Deserialize(string json) => new TestRecord { Id = 42, Name = "from-custom" };
    }

    private class TestRecordSerializer : ISerializer<TestRecord>
    {
        public string Serialize(TestRecord obj) => $"{{\"Id\":{obj.Id},\"Name\":\"{obj.Name}\"}}";
        public TestRecord Deserialize(string json) => new TestRecord { Id = 7, Name = "from-custom" };
    }
}
