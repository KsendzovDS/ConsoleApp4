using ConsoleApp4;

namespace Lab3
{
    class Program
    {
        static void Main()
        {
            string input = "Привет, это тестовый текст!Проверка связи. всех! недостающих знаков препинания? 'Ах, господи': - сказал он.";

            Console.WriteLine("=== Исходный текст ===");
            Console.WriteLine(input);
            Console.WriteLine();

            TextParser parser = new TextParser(input);
            Text text = parser.Parse();

            Console.WriteLine("=== Результат парсинга ===");
            foreach (var sentence in text.Sentences)
                Console.WriteLine(sentence);
            Console.WriteLine($"\nВсего предложений: {text.Sentences.Count}");
            Console.WriteLine();

            Console.WriteLine("=== Сортировка предложений по количеству слов ===");
            var byWords = text.GetSentencesByWordCount();
            foreach (var s in byWords)
                Console.WriteLine($"[{s.WordCount} слов] {s}");
            Console.WriteLine();

            Console.WriteLine("=== Сортировка предложений по длине ===");
            var byLength = text.GetSentencesByLength();
            foreach (var s in byLength)
                Console.WriteLine($"[{s.ToString().Length} символов] {s}");
            Console.WriteLine();

            Console.WriteLine("=== Слова длиной 5 в вопросительных предложениях ===");
            var questionWords = text.FindWordsInQuestions(5);
            if (questionWords.Count > 0)
                Console.WriteLine(string.Join(", ", questionWords));
            else
                Console.WriteLine("Нет таких слов.");
            Console.WriteLine();

            Console.WriteLine("=== Удаляем слова длиной 4, начинающиеся с согласной ===");
            text.RemoveWordsStartingWithConsonant(4);
            foreach (var s in text.Sentences)
                Console.WriteLine(s);
            Console.WriteLine();

            Console.WriteLine("=== Заменяем слова длиной 5 на '#####' во 2 предложении ===");
            text.ReplaceWordsInSentence(1, 5, "#####");
            foreach (var s in text.Sentences)
                Console.WriteLine(s);
            Console.WriteLine();

            Console.WriteLine("=== Удаляем стоп-слова ===");
            var stopWords = new HashSet<string> { "это", "как", "наш" };
            text.RemoveStopWords(stopWords);
            foreach (var s in text.Sentences)
                Console.WriteLine(s);
            Console.WriteLine();

            Console.WriteLine("=== Программа завершена ===");
        }
    }
}
