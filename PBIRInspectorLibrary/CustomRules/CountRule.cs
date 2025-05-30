﻿using Json.Logic;
using Json.More;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace PBIRInspectorLibrary.CustomRules
{
    /// <summary>
    /// Handles the `count` operation.
    /// </summary>
    [Operator("count")]
    [JsonConverter(typeof(CountJsonConverter))]
    public class CountRule : Json.Logic.Rule
    {
        internal Json.Logic.Rule Input { get; }

        public CountRule(Json.Logic.Rule input)
        {
            Input = input;
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
            int result = 0;
            var input = Input.Apply(data, contextData);

            if (input is null) return result;
            if (input is not JsonArray arr)
                throw new JsonLogicException($"Cannot count items in non-JsonArray object.");

            result = arr.Count;

            return result;
        }
    }


    internal class CountJsonConverter : WeaklyTypedJsonConverter<CountRule>
    {
        public override CountRule? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var parameters = reader.TokenType == JsonTokenType.StartArray
            ? options.ReadArray(ref reader, PBIRInspectorSerializerContext.Default.Rule)
            : new[] { options.Read(ref reader, PBIRInspectorSerializerContext.Default.Rule)! };

            if (parameters == null || parameters.Length == 0)
                throw new JsonException("The count rule needs an array of parameters.");

            return new CountRule(parameters[0]);
        }

        public override void Write(Utf8JsonWriter writer, CountRule value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
