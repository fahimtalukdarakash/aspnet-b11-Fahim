using System.Collections;
using System.Reflection;
using System.Text;

namespace XmlFormattingAssignment
{
    public static class XmlFormatter
    {
        public static string Convert(object obj)
        {
            if (obj == null)
                return string.Empty;

            Type type = obj.GetType();
            StringBuilder xmlBuilder = new StringBuilder();

            string rootName = type.Name;
            xmlBuilder.Append($"<{rootName}>");
            ProcessObject(obj, type, xmlBuilder);
            xmlBuilder.Append($"</{rootName}>");
            //Console.WriteLine(xmlBuilder.ToString());
            return xmlBuilder.ToString();
        }

        private static void ProcessObject(object obj, Type type, StringBuilder xmlBuilder)
        {
            foreach (PropertyInfo property in type.GetProperties())
            {
                object? value = property.GetIndexParameters().Length == 0
                    ? property.GetValue(obj)
                    : null; // Skip indexers

                string propName = property.Name;

                if (value == null)
                {
                    xmlBuilder.Append($"<{propName}></{propName}>");
                }
                else if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                {
                    xmlBuilder.Append($"<{propName}>{value}</{propName}>");
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    xmlBuilder.Append($"<{propName}>{((DateTime)value).ToString("M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture)}</{propName}>");
                }
                else if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                {
                    Type? underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                    if (underlyingType == typeof(DateTime))
                    {
                        xmlBuilder.Append(value != null
                            ? $"<{propName}>{((DateTime)value).ToString("M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture)}</{propName}>"
                            : $"<{propName}></{propName}>");
                    }
                    else
                    {
                        xmlBuilder.Append(value != null
                            ? $"<{propName}>{value}</{propName}>"
                            : $"<{propName}></{propName}>");
                    }
                }
                else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
                {
                    xmlBuilder.Append($"<{propName}>");
                    foreach (var item in (IEnumerable)value)
                    {
                        ProcessItem(item, xmlBuilder);
                    }
                    xmlBuilder.Append($"</{propName}>");
                }
                else
                {
                    xmlBuilder.Append($"<{propName}>");
                    ProcessObject(value, property.PropertyType, xmlBuilder);
                    xmlBuilder.Append($"</{propName}>");
                }
            }
        }




        private static void ProcessItem(object item, StringBuilder xmlBuilder)
        {
            if (item == null) return;

            if (item is string)
            {
                // Treat string as a value, not a collection
                xmlBuilder.Append($"<String>{item}</String>");
            }
            else
            {
                Type itemType = item.GetType();
                string itemName = itemType.Name;

                xmlBuilder.Append($"<{itemName}>");
                ProcessObject(item, itemType, xmlBuilder);
                xmlBuilder.Append($"</{itemName}>");
            }
        }
    }
}