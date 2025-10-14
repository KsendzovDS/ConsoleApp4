namespace ConsoleApp4
{
    class Program
    {
        static void Main()
        {
            string input = "Привет, это тестовый текст. Необходимый для проверки моей программы на работтоспособность";
            Text text = TextParser.Parse(input);

            Console.WriteLine("=== Все предложения ===");
            foreach (var sentence in text.Sentences)
                Console.WriteLine(sentence.ToString());

            Console.WriteLine("\nСтруктура текста");
            Console.WriteLine($"Предложений: {text.Sentences.Count}");
            for (int i = 0; i < text.Sentences.Count; i++)
                Console.WriteLine($"Предложение {i + 1}, слов: {text.Sentences[i].WordCount}");
        }
    }
}
