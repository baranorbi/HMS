using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HMS.Shared.Converters
{
    public class CollectionJsonConverter : JsonConverter<List<int>>
    {
        public override List<int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Default empty list
            var result = new List<int>();

            if (reader.TokenType == JsonTokenType.Null)
                return result;

            // Handle direct array format
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                return JsonSerializer.Deserialize<List<int>>(ref reader, options);
            }

            // Handle nested object format with $values property
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                using var jsonDoc = JsonDocument.ParseValue(ref reader);
                var root = jsonDoc.RootElement;

                // Check if $values property exists
                if (root.TryGetProperty("$values", out var valuesProperty))
                {
                    // Parse the $values array
                    if (valuesProperty.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var item in valuesProperty.EnumerateArray())
                        {
                            if (item.ValueKind == JsonValueKind.Number)
                            {
                                result.Add(item.GetInt32());
                            }
                        }
                    }
                }
                return result;
            }

            throw new JsonException($"Unexpected JSON token: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, List<int> value, JsonSerializerOptions options)
        {
            // This handles serializing the list back to JSON
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}