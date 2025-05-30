﻿using Json.Logic;
using Json.More;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace PBIRInspectorLibrary.CustomRules
{
    /// <summary>
    /// Handles the `strcontains` operation.
    /// </summary>
    [Operator("strcontains")]
    [JsonConverter(typeof(StringContainsJsonConverter))]
    public class StringContains : Json.Logic.Rule
    {
        internal Json.Logic.Rule SearchString { get; }
        internal Json.Logic.Rule ContainsString { get; }

        public StringContains(Json.Logic.Rule searchString, Json.Logic.Rule containsString)
        {
            SearchString = searchString;
            ContainsString = containsString;
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
            var searchString = SearchString.Apply(data, contextData);
            var containsString = ContainsString.Apply(data, contextData);

            if (searchString == null) return 0;
            
            if (searchString is not JsonValue searchStringValue || !searchStringValue.TryGetValue(out string? stringSearchString))
                throw new JsonLogicException($"strcontains rule: searchString parameter value is not a string.");

            if (containsString is not JsonValue containsStringValue || !containsStringValue.TryGetValue(out string? stringContainsString))
                throw new JsonLogicException($"strcontains rule: containsString parameter value is not a string.");

            return Regex.Matches(stringSearchString, stringContainsString).Count;
        }
    }

    internal class StringContainsJsonConverter : WeaklyTypedJsonConverter<StringContains>
    {
        public override StringContains? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var parameters = reader.TokenType == JsonTokenType.StartArray
           ? options.ReadArray(ref reader, PBIRInspectorSerializerContext.Default.Rule)
           : new[] { options.Read(ref reader, PBIRInspectorSerializerContext.Default.Rule)! };

            if (parameters is not { Length: 2 })
                throw new JsonException("The strcontains rule needs an array with 2 parameters.");

            if (parameters.Length == 2) return new StringContains(parameters[0], parameters[1]);

            return new StringContains(parameters[0], parameters[1]);
        }

        public override void Write(Utf8JsonWriter writer, StringContains value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
            //writer.WriteStartObject();
            //writer.WritePropertyName("strcontains");
            //writer.WriteStartArray();
            //writer.WriteRule(value.SearchString, options);
            //writer.WriteRule(value.ContainsString, options);

            //writer.WriteEndArray();
            //writer.WriteEndObject();
        }
    }
}