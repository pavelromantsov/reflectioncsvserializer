using System.Reflection;

namespace ReflectionCsvSerializer
{
    public static class CsvReflectionSerializer
    {
        // Dictionary для хранения информации о полях
        private static readonly Dictionary<Type, FieldInfo[]> _fieldCache = new();

        private static FieldInfo[] GetCachedFields<T>()
        {
            var type = typeof(T);
            if (!_fieldCache.TryGetValue(type, out var fields))
            {
                // Сортируем, чтобы порядок полей при сериализации и десериализации совпадал
                fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                             .OrderBy(f => f.Name)
                             .ToArray();
                _fieldCache[type] = fields;
            }
            return fields;
        }
        public static string Serialize<T>(T obj)
        {
            if (obj == null) return string.Empty;

            var fields = GetCachedFields<T>();
            var values = new string[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                values[i] = fields[i].GetValue(obj)?.ToString() ?? string.Empty;
            }

          return string.Join(",", values);
        }

        public static T Deserialize<T>(string csv) where T : new()
        {
            if (string.IsNullOrEmpty(csv)) return new T();

            var obj = new T();
            var fields = GetCachedFields<T>();
            var values = csv.Split(',');

            for (int i = 0; i < fields.Length && i < values.Length; i++)
            {
                var fieldType = fields[i].FieldType;        
                var targetType = Nullable.GetUnderlyingType(fieldType) ?? fieldType;

                if (!string.IsNullOrEmpty(values[i]))
                {
                    try
                    {
                        var safeValue = Convert.ChangeType(values[i], targetType);
                        fields[i].SetValue(obj, safeValue);
                    }
                    catch (InvalidCastException)
                    {                    
                    }
                }
            }
            return obj;
        }
    }
}
