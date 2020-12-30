using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

// BORROWED FROM https://geeks.ms/jorge/2020/03/18/cannot-get-the-value-of-a-token-type-number-as-a-string-con-system-text-json/
public class LongToStringJsonConverter : JsonConverter<string>
{
    public LongToStringJsonConverter() { }

    public override string Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number && 
            type == typeof(String))
            return reader.GetString();

        var span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

        if (Utf8Parser.TryParse(span, out long number, out var bytesConsumed) && span.Length == bytesConsumed)
            return number.ToString();

        var data = reader.GetString();

        throw new InvalidOperationException($"'{data}' is not a correct expected value!")
        {
            Source = "LongToStringJsonConverter"
        };
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}