using System.Diagnostics;
using System.Text.Json;

namespace ReflectionCsvSerializer
{
    public static class Program
    {
        static void Main()
        {
            const int iterations = 100000;
            var instance = F.Get();
            
            var csvData = CsvReflectionSerializer.Serialize(instance);

            var sw = Stopwatch.StartNew();

            // Мой Reflection: Сериализация
            for (int i = 0; i < iterations; i++)
            {
                var _ = CsvReflectionSerializer.Serialize(instance);
            }
            sw.Stop();
            long mySerializeTime = sw.ElapsedMilliseconds;

            // Мой Reflection: Десериализация
            sw.Restart();
            F? myDeserialized = null;
            for (int i = 0; i < iterations; i++)
            {
                myDeserialized = CsvReflectionSerializer.Deserialize<F>(csvData);
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
            F? stdDeserialized = null;
            for (int i = 0; i < iterations; i++)
            {
                stdDeserialized = JsonSerializer.Deserialize<F>(jsonData, jsonOptions);
            }
            sw.Stop();
            long stdDeserializeTime = sw.ElapsedMilliseconds;

            // Измерение времени вывода в консоль
            sw.Restart();
            Console.WriteLine("Тестовый вывод в консоль для замера времени");
            sw.Stop();
            long consoleWriteTime = sw.ElapsedMilliseconds;

            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Сериализуемый класс: class F { public int I1, I2, I3, I4, I5; }");
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

            Console.WriteLine("\n--- Проверка корректности десериализации ---");
            Console.WriteLine($"Исходный объект:       I1={instance.I1}, I2={instance.I2}, " +
                $"I3={instance.I3}, I4={instance.I4}, I5={instance.I5}");
            Console.WriteLine($"CSV строка: {csvData}");
            Console.WriteLine($"JSON строка: {jsonData}");
            Console.WriteLine($"Reflection (CSV):      I1={myDeserialized?.I1}, " +
                $"I2={myDeserialized?.I2}, I3={myDeserialized?.I3}, I4={myDeserialized?.I4}," +
                $" I5={myDeserialized?.I5}");
            Console.WriteLine($"System.Text.Json:      I1={stdDeserialized?.I1}, " +
                $"I2={stdDeserialized?.I2}, I3={stdDeserialized?.I3}, " +
                $"I4={stdDeserialized?.I4}, I5={stdDeserialized?.I5}");
            Console.WriteLine(new string('-', 50));

            // Демонстрация работы
            Console.WriteLine("\nДемонстрация работы:");
            Console.WriteLine($"Исходный объект: I1={instance.I1}, I2={instance.I2}, I3={instance.I3}," +
                $" I4={instance.I4}, I5={instance.I5}");
            Console.WriteLine($"CSV строка: {csvData}");
            var deserialized = CsvReflectionSerializer.Deserialize<F>(csvData);
            Console.WriteLine($"Восстановленный объект: I1={deserialized.I1}, I2={deserialized.I2}, " +
                $"I3={deserialized.I3}, I4={deserialized.I4}, I5={deserialized.I5}");
        }
    }
}
