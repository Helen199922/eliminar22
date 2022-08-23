using System.Text.Json;
using System.Text.Json.Serialization;

namespace CarniceriaFinal.Core.TimesConverts
{
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
	{
		public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return TimeOnly.Parse(reader.GetString()!);
		}

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
			var isoDate = value.ToString("O");
			writer.WriteStringValue(isoDate);
		}
    }
}
