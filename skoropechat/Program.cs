using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml;

public class Record
{
    public string Name { get; set; }
    public int CharactersPerMinute { get; set; }
    public int CharactersPerSecond { get; set; }
}

public class TypingTest
{
    private const string recordsFilePath = "records.json";
    private static List<Record> records = new List<Record>();

    public static void Main()
    {
        LoadRecords();
        bool repeatTest = true;

        while (repeatTest)
        {
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();

            string textToType = "Книга посвящена приключениям молодого дворянина по имени д'Артаньян, отправившегося в Париж, чтобы стать мушкетёром, и трёх его друзей-мушкетёров Атоса, Портоса и Арамиса в период между 1625 и 1628 годами. Нажмите Enter, чтобы начать.";
            Console.WriteLine(textToType);
            Console.ReadLine();

            int typedIndex = 0;
            Console.WriteLine(">> " + textToType.Substring(0, typedIndex) + "[" + textToType.Substring(typedIndex) + "]");

            Stopwatch stopwatch = Stopwatch.StartNew();

            bool completed = false;
            while (!completed)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                char keyChar = key.KeyChar;

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (typedIndex > 0)
                    {
                        typedIndex--;
                    }
                }
                else
                {
                    if (typedIndex < textToType.Length && keyChar == textToType[typedIndex])
                    {
                        typedIndex++;
                    }
                }

                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine(">> " + textToType.Substring(0, typedIndex) + "[" + textToType.Substring(typedIndex) + "]");

                if (typedIndex == textToType.Length)
                {
                    completed = true;
                }
            }

            stopwatch.Stop();
            double totalTimeSeconds = stopwatch.Elapsed.TotalSeconds;

            Record currentRecord = new Record
            {
                Name = name,
                CharactersPerMinute = (int)(textToType.Length / (totalTimeSeconds / 60)),
                CharactersPerSecond = (int)(textToType.Length / totalTimeSeconds)
            };

            records.Add(currentRecord);
            SaveRecords();
            DisplayRecords();

            Console.Write("Хотите пройти тест еще раз? (Да/Нет): ");
            repeatTest = Console.ReadLine().Trim().Equals("Да", StringComparison.OrdinalIgnoreCase);
        }
    }

    private static void LoadRecords()
    {
        if (File.Exists(recordsFilePath))
        {
            string json = File.ReadAllText(recordsFilePath);
            records = JsonConvert.DeserializeObject<List<Record>>(json);
        }
    }

    private static void SaveRecords()
    {
        string json = JsonConvert.SerializeObject(records, Formatting.Indented);
        File.WriteAllText(recordsFilePath, json);
    }

    private static void DisplayRecords()
    {
        Console.WriteLine("---- Таблица рекордов ----");
        foreach (var record in records)
        {
            Console.WriteLine($"Имя: {record.Name}, Знаков в минуту: {record.CharactersPerMinute}, Знаков в секунду: {record.CharactersPerSecond}");
        }
        Console.WriteLine("-------------------------");
    }
}