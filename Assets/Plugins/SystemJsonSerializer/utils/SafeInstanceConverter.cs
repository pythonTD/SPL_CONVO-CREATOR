using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace System.Text.Json
{
  public class SafeInstanceConverter<T> : JsonConverter<T>
  {
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      try
      {
        return JsonSerializer.Deserialize<T>(ref reader, options);
      }
      catch (JsonException)
      {
        return GetDefaultValue();
      }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
      JsonSerializer.Serialize(writer, value, options);
    }

    private T GetDefaultValue()
    {
      if (typeof(T) == typeof(string)) return (T)(object)string.Empty;
      if (typeof(T).IsValueType) return Activator.CreateInstance<T>();
      if (typeof(T) == typeof(bool)) return (T)(object)false;
      if (typeof(T) == typeof(int)) return (T)(object)0;
      if (typeof(T) == typeof(double)) return (T)(object)0.0;
      if (typeof(T) == typeof(float)) return (T)(object)0f;
      if (typeof(T) == typeof(decimal)) return (T)(object)0m;

      return (T)Activator.CreateInstance(typeof(T));
    }
  }
}