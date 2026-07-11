using System;
using System.Collections.Concurrent;
using System.Text.Json;
using Genesis.Engine.Core.Logging;

namespace Genesis.Engine.Core.Runtime.Serialization
{
    public class SerializationManager
    {
        private readonly ConcurrentDictionary<Type, object> serializers = new();

        public void Register<T>(ISerializer<T> serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            serializers[typeof(T)] = serializer!;
            Logger.Info($"SerializationManager: Registered serializer for {typeof(T).FullName}");
        }

        public bool TryGetSerializer<T>(out ISerializer<T>? serializer)
        {
            if (serializers.TryGetValue(typeof(T), out var obj) && obj is ISerializer<T> s)
            {
                serializer = s;
                return true;
            }

            serializer = default;
            return false;
        }

        public bool TryGetSerializer(Type type, out object? serializer)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (serializers.TryGetValue(type, out var obj))
            {
                serializer = obj;
                return true;
            }

            serializer = null;
            return false;
        }

        /// <summary>
        /// Serialize with priority: registered ISerializer&lt;T&gt; -> System.Text.Json fallback
        /// </summary>
        public string Serialize<T>(T data)
        {
            if (TryGetSerializer<T>(out var serializer) && serializer != null)
            {
                return serializer.Serialize(data);
            }

            // Fallback to System.Text.Json
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false
            };
            try
            {
                return JsonSerializer.Serialize(data, options);
            }
            catch (Exception ex)
            {
                Logger.Warn($"SerializationManager: Fallback serialize failed for {typeof(T).FullName}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deserialize with priority: registered ISerializer&lt;T&gt; -> System.Text.Json fallback
        /// </summary>
        public T Deserialize<T>(string json)
        {
            if (TryGetSerializer<T>(out var serializer) && serializer != null)
            {
                return serializer.Deserialize(json);
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                return JsonSerializer.Deserialize<T>(json, options)!;
            }
            catch (Exception ex)
            {
                Logger.Warn($"SerializationManager: Fallback deserialize failed for {typeof(T).FullName}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Non-generic serialize for runtime types. Tries registered serializer first, then System.Text.Json.
        /// </summary>
        public string Serialize(object data, Type type)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (TryGetSerializer(type, out var serObj) && serObj != null)
            {
                // Attempt to invoke ISerializer<T>.Serialize via reflection
                var method = serObj.GetType().GetMethod("Serialize");
                if (method != null)
                {
                    try
                    {
                        return (string)method.Invoke(serObj, new object[] { data })!;
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn($"SerializationManager: Registered serializer invocation failed for {type.FullName}: {ex.Message}");
                        // fall through to fallback
                    }
                }
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false
            };
            try
            {
                return JsonSerializer.Serialize(data, type, options);
            }
            catch (Exception ex)
            {
                Logger.Warn($"SerializationManager: Fallback serialize failed for {type.FullName}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Non-generic deserialize for runtime types. Tries registered serializer first, then System.Text.Json.
        /// </summary>
        public object Deserialize(string json, Type type)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (TryGetSerializer(type, out var serObj) && serObj != null)
            {
                var method = serObj.GetType().GetMethod("Deserialize");
                if (method != null)
                {
                    try
                    {
                        return method.Invoke(serObj, new object[] { json })!;
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn($"SerializationManager: Registered serializer invocation failed for {type.FullName}: {ex.Message}");
                        // fall through to fallback
                    }
                }
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                return JsonSerializer.Deserialize(json, type, options)!;
            }
            catch (Exception ex)
            {
                Logger.Warn($"SerializationManager: Fallback deserialize failed for {type.FullName}: {ex.Message}");
                throw;
            }
        }
    }
}
