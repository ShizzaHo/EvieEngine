using System;
using System.Collections.Generic;
using System.Text;

namespace Eviecore
{
    public static class DictionaryJSON
    {
        public static string Serialize(Dictionary<string, object> dictionary)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");

            bool isFirst = true;
            foreach (var kvp in dictionary)
            {
                if (!isFirst)
                {
                    jsonBuilder.Append(",");
                }

                string key = EscapeString(kvp.Key);
                string value = SerializeValue(kvp.Value);

                jsonBuilder.AppendFormat("\"{0}\":{1}", key, value);
                isFirst = false;
            }

            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        private static string SerializeValue(object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is string stringValue)
            {
                return $"\"{EscapeString(stringValue)}\"";
            }

            if (value is bool boolValue)
            {
                return boolValue ? "true" : "false";
            }

            if (value is int || value is float || value is double || value is long || value is decimal)
            {
                return value.ToString();
            }

            if (value is Dictionary<string, object> nestedDictionary)
            {
                return Serialize(nestedDictionary);
            }

            throw new NotSupportedException($"Type {value.GetType()} is not supported for serialization.");
        }

        private static string EscapeString(string input)
        {
            return input.Replace("\\", "\\\\")
                        .Replace("\"", "\\\"")
                        .Replace("\n", "\\n")
                        .Replace("\r", "\\r")
                        .Replace("\t", "\\t");
        }

        public static Dictionary<string, object> Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException("Input JSON is null or empty.");
            }

            int index = 0;
            SkipWhitespace(json, ref index);

            if (json[index] != '{')
            {
                throw new FormatException("JSON must start with '{'.");
            }

            index++; // Skip '{'
            var dictionary = new Dictionary<string, object>();

            while (index < json.Length)
            {
                SkipWhitespace(json, ref index);

                if (json[index] == '}')
                {
                    index++; // End of object
                    break;
                }

                if (dictionary.Count > 0)
                {
                    if (json[index] != ',')
                    {
                        throw new FormatException("Expected ',' between key-value pairs.");
                    }
                    index++; // Skip ','
                }

                SkipWhitespace(json, ref index);

                string key = ParseString(json, ref index);
                SkipWhitespace(json, ref index);

                if (json[index] != ':')
                {
                    throw new FormatException("Expected ':' after key.");
                }

                index++; // Skip ':'
                SkipWhitespace(json, ref index);

                object value = ParseValue(json, ref index);
                dictionary[key] = value;
            }

            return dictionary;
        }

        private static string ParseString(string json, ref int index)
        {
            if (json[index] != '"')
            {
                throw new FormatException("Expected '\"' at the beginning of a string.");
            }

            index++; // Skip '"'
            int start = index;

            while (index < json.Length && json[index] != '"')
            {
                if (json[index] == '\\')
                {
                    index++; // Skip escape character
                }
                index++;
            }

            if (index >= json.Length)
            {
                throw new FormatException("Unterminated string.");
            }

            string result = json.Substring(start, index - start);
            index++; // Skip closing '"'
            return result.Replace("\\\"", "\"")
                         .Replace("\\n", "\n")
                         .Replace("\\r", "\r")
                         .Replace("\\t", "\t")
                         .Replace("\\\\", "\\");
        }

        private static object ParseValue(string json, ref int index)
        {
            SkipWhitespace(json, ref index);

            if (json[index] == '"')
            {
                return ParseString(json, ref index);
            }

            if (json[index] == '{')
            {
                return Deserialize(json.Substring(index));
            }

            if (json[index] == 't' && json.Substring(index, 4) == "true")
            {
                index += 4;
                return true;
            }

            if (json[index] == 'f' && json.Substring(index, 5) == "false")
            {
                index += 5;
                return false;
            }

            if (json[index] == 'n' && json.Substring(index, 4) == "null")
            {
                index += 4;
                return null;
            }

            int start = index;
            while (index < json.Length && (char.IsDigit(json[index]) || json[index] == '.' || json[index] == '-'))
            {
                index++;
            }

            string number = json.Substring(start, index - start);
            if (number.Contains("."))
            {
                return double.Parse(number);
            }

            return int.Parse(number);
        }

        private static void SkipWhitespace(string json, ref int index)
        {
            while (index < json.Length && char.IsWhiteSpace(json[index]))
            {
                index++;
            }
        }
    }
}