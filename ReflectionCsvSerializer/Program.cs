using System.Diagnostics;
using System.Text.Json;

namespace ReflectionCsvSerializer
{
    public static class Program
    {
        static void Main()
        {
            int iterations = 100000;
            var instance = F.Get();
            var csvData = CsvReflectionSerializer.Serialize(instance);


            // Мой Reflection: Сериализация
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var _ = CsvReflectionSerializer.Serialize(instance);
            }
            sw.Stop();
            long mySerializeTime = sw.ElapsedMilliseconds;

            // Мой Reflection: Десериализация
            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                var _ = CsvReflectionSerializer.Deserialize<F>(csvData);
            }
            sw.Stop();
            long myDeserializeTime = sw.ElapsedMilliseconds;

            // Стандартный механизм (System.Text.Json): Сериализация
            var jsonOptions = new JsonSerializerOptions { WriteIndented = false };
            string jsonData = JsonSerializer.Serialize(instance, jsonOptions);

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                var _ = JsonSerializer.Serialize(instance, jsonOptions);
            }
            sw.Stop();
            long stdSerializeTime = sw.ElapsedMilliseconds;

            // Стандартный механизм (System.Text.Json): Десериализация
            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                var _ = JsonSerializer.Deserialize<F>(jsonData, jsonOptions);
            }
            sw.Stop();
            long stdDeserializeTime = sw.ElapsedMilliseconds;

            // Измерение времени вывода в консоль
            sw.Restart();
            Console.WriteLine("Тестовый вывод в консоль для замера времени");
            sw.Stop();
            long consoleWriteTime = sw.ElapsedMilliseconds;

            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Сериализуемый класс: class F { public int i1, i2, i3, i4, i5; }");
            Console.WriteLine("код сериализации-десериализации: в файле CsvReflectionSerializer.cs");
            Console.WriteLine($"количество замеров: {iterations} итераций");
            Console.WriteLine("мой рефлекшен:");
            Console.WriteLine($"Время на сериализацию = {mySerializeTime} мс");
            Console.WriteLine($"Время на десериализацию = {myDeserializeTime} мс");
            Console.WriteLine("стандартный механизм (System.Text.Json):");
            Console.WriteLine($"Время на сериализацию = {stdSerializeTime} мс");
            Console.WriteLine($"Время на десериализацию = {stdDeserializeTime} мс");
            Console.WriteLine($"Время на вывод текста в консоль = {consoleWriteTime} мс");
            Console.WriteLine(new string('-', 50));

            // Демонстрация работы
            Console.WriteLine("\nДемонстрация работы:");
            Console.WriteLine($"Исходный объект: i1={instance.i1}, i2={instance.i2}, i3={instance.i3}, i4={instance.i4}, i5={instance.i5}");
            Console.WriteLine($"CSV строка: {csvData}");
            var deserialized = CsvReflectionSerializer.Deserialize<F>(csvData);
            Console.WriteLine($"Восстановленный объект: i1={deserialized.i1}, i2={deserialized.i2}, i3={deserialized.i3}, i4={deserialized.i4}, i5={deserialized.i5}");
        }
    }
}
