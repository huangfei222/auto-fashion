using System;
using System.Text.Json;

namespace Genesis.Engine.Core.Runtime.Serialization
{
    public class SaveDataJsonSerializer : ISerializer<Genesis.Engine.Core.Runtime.Persistence.SaveData>
    {
        private readonly JsonSerializerOptions options;

        public SaveDataJsonSerializer()
        {
            options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        public string Serialize(Genesis.Engine.Core.Runtime.Persistence.SaveData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return JsonSerializer.Serialize(data, options);
        }

        public Genesis.Engine.Core.Runtime.Persistence.SaveData Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) throw new ArgumentNullException(nameof(json));
            return JsonSerializer.Deserialize<Genesis.Engine.Core.Runtime.Persistence.SaveData>(json, options)
                   ?? throw new InvalidOperationException("Deserialize returned null for SaveData");
        }
    }
}
