using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Serialises an object into JSON or XML.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="value"></param>
/// <param name="type"></param>
/// <returns></returns>
public static string Serialize<T>(T value, SerializeType type)
{
    switch (type)
    {
        case SerializeType.Xml:
            if (value == null) return string.Empty;

            var xmlserializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, value);
                return stringWriter.ToString();
            }

        case SerializeType.Json:

            var result = JsonConvert.SerializeObject(value);
            return result;

        default:
            return string.Empty;
    }
}

public enum SerializeType
{
    [Description("application/json")]
    Json,
    [Description("text/xml")]
    Xml
}