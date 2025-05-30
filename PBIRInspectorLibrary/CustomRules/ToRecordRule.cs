﻿using Json.Logic;
using Json.More;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace PBIRInspectorLibrary.CustomRules
{
    /// <summary>
    /// Handles the `torecord` operation.
    /// </summary>
    [Operator("torecord")]
    [JsonConverter(typeof(ToRecordRuleJsonConverter))]
    public class ToRecordRule : Json.Logic.Rule
    {
        internal List<Json.Logic.Rule> Items { get; }

        internal ToRecordRule(Json.Logic.Rule a, params Json.Logic.Rule[] more)
        {
            Items = new List<Json.Logic.Rule> { a };
            Items.AddRange(more);
        }

        /// <summary>
        /// Applies the rule to the input data.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <param name="contextData">
        ///     Optional secondary data.  Used by a few operators to pass a secondary
        ///     data context to inner operators.
        /// </param>
        /// <returns>The result of the rule.</returns>
        public override JsonNode? Apply(JsonNode? data, JsonNode? contextData = null)
        {
            JsonObject result = new();

            var count = Items.Count;

            if (count % 2 != 0) { throw new JsonLogicException("The torecord rule expects an even number of paramaters"); }

            for (var i = 0; i < count - 1; i += 2)
            {
                var key = Items[i];
                var keyValue = key.Apply(data, contextData).Stringify();
                var item = Items[i + 1];
                var value = item.Apply(data, contextData);
                result.Add(keyValue, value?.DeepClone());
            }

            return result;
        }

        internal class ToRecordRuleJsonConverter : WeaklyTypedJsonConverter<ToRecordRule>
        {
            public override ToRecordRule? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var parameters = reader.TokenType == JsonTokenType.StartArray
                   ? options.ReadArray(ref reader, PBIRInspectorSerializerContext.Default.Rule)
                   : new[] { options.Read(ref reader, PBIRInspectorSerializerContext.Default.Rule)! };

                if (parameters == null || parameters.Length == 0)
                    throw new JsonException("The torecord rule needs an array of parameters.");

                return new ToRecordRule(parameters[0], parameters.Skip(1).ToArray());
            }

            public override void Write(Utf8JsonWriter writer, ToRecordRule value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
                //writer.WriteStartObject();
                //writer.WritePropertyName("torecord");
                //writer.WriteRules(value.Items, options);
                //writer.WriteEndObject();
            }
        }
    }
}