using ConsoleApp4;

namespace ConsoleApp4
{
    class Program
    {
        static void Main()
        {
            string input = "Привет, это тестовый текст!Проверка связи. всех! недостающих знаков препинания? 'Ах, господи': - сказал он. Оно, они существуют. always. allow ";

            Console.WriteLine("=== Исходный текст ===");
            Console.WriteLine(input);
            Console.WriteLine();

            TextParser parser = new TextParser(input);
            Text text = parser.Parse();

            while (true)
            {
                Console.WriteLine("\n=== ВЫБЕРИТЕ ОПЕРАЦИЮ ===");
                Console.WriteLine("1. Сортировка по количеству слов");
                Console.WriteLine("2. Сортировка по длине предложений");
                Console.WriteLine("3. Слова в вопросительных предложениях");
                Console.WriteLine("4. Удалить слова с согласной буквы");
                Console.WriteLine("5. Заменить слова в предложении");
                Console.WriteLine("6. Удалить стоп-слова");
                Console.WriteLine("7. Экспорт в xml");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("=== Сортировка по количеству слов ===");
                        var byWords = text.GetSentencesByWordCount();
                        foreach (var s in byWords)
                            Console.WriteLine($"[{s.WordCount} слов] {s}");
                        break;

                    case "2":
                        Console.WriteLine("=== Сортировка по длине ===");
                        var byLength = text.GetSentencesByLength();
                        foreach (var s in byLength)
                            Console.WriteLine($"[{s.ToString().Length} символов] {s}");
                        break;

                    case "3":
                        Console.WriteLine("=== Слова в вопросительных предложениях ===");
                        Console.Write("Введите длину слова: ");
                        int length = Convert.ToInt32(Console.ReadLine());
                        var questionWords = text.FindWordsInQuestions(length);
                        if (questionWords.Count > 0)
                            Console.WriteLine(string.Join(", ", questionWords));
                        else
                            Console.WriteLine("Нет таких слов.");
                        break;

                    case "4":
                        Console.WriteLine("=== Удаляем слова с согласной буквы ===");
                        Console.Write("Введите длину слова: ");
                        int len = Convert.ToInt32(Console.ReadLine());
                        text.RemoveWordsStartingWithConsonant(len);
                        foreach (var s in text.Sentences)
                            Console.WriteLine(s);
                        break;

                    case "5":
                        Console.WriteLine("=== Замена слов в предложении ===");
                        Console.Write("Введите номер предложения: ");
                        int sentIndex = Convert.ToInt32(Console.ReadLine()) - 1;
                        Console.Write("Введите длину слов для замены: ");
                        int wordLen = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Введите строку для замены: ");
                        string replacement = Console.ReadLine();
                        text.ReplaceWordsInSentence(sentIndex, wordLen, replacement);
                        foreach (var s in text.Sentences)
                            Console.WriteLine(s);
                        break;

                    case "6":
                        Console.WriteLine("Выберите:\n" +
                            "1. Русские стоп-слова\n" +
                            "2. Английские стоп-слова");
                        int c = Convert.ToInt32(Console.ReadLine());
                        string p = "";
                        switch (c)
                        {
                            case 1:
                                p = @"C:\Users\PC\source\repos\ConsoleApp4\ConsoleApp4\stopwords_ru.txt";
                                break;
                            case 2:
                                p = @"C:\Users\PC\source\repos\ConsoleApp4\ConsoleApp4\stopwords_en.txt";
                                break;
                            default:
                                Console.WriteLine("Неверный выбор!");
                                continue;
                        }
                        Text.RemoveStopWords(text, p);
                        Console.WriteLine("Результат:");
                        foreach (var s in text.Sentences)
                            Console.WriteLine(s);
                        break;

                    case "7":
                        Console.WriteLine("=== Экспорт в XML ===");
                        Console.Write("Введите путь для сохранения: ");
                        string exportPath = Console.ReadLine();
                        text.ExportToXml(exportPath);
                        Console.WriteLine($"Текст экспортирован в {exportPath}");
                        break;

                    case "0":
                        Console.WriteLine("Выход из программы.");
                        return;

                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }
    }
}