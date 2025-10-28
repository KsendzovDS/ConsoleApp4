using ConsoleApp4;
using System.Text;

public class TextParser
{
    private readonly string input;
    private static readonly char[] SentenceEndings = { '.', '?', '!' };
    private static readonly HashSet<char> PunctChars = new()
    {
        '.', ',', ';', ':', '?', '!', '—', '-', '(', ')', '[', ']', '{', '}', '«', '»', '"', '\'', '…'
    };

    public TextParser(string text)
    {
        input = text ?? string.Empty;
    }

    public Text Parse()
    {
        var text = new Text();
        var sentence = new Sentence();
        var wordBuilder = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            char ch = input[i];

            if (char.IsLetterOrDigit(ch) || ch == '\'')
            {
                wordBuilder.Append(ch);
            }
            else
            {
                if (wordBuilder.Length > 0)
                {
                    sentence.AddToken(new Word(wordBuilder.ToString()));
                    wordBuilder.Clear();
                }

                if (PunctChars.Contains(ch))
                {
                    var punc = new StringBuilder().Append(ch);
                    while (i + 1 < input.Length && PunctChars.Contains(input[i + 1]))
                    {
                        i++;
                        punc.Append(input[i]);
                    }

                    sentence.AddToken(new Punctuation(punc.ToString()));
                    if (punc.ToString().Any(c => SentenceEndings.Contains(c)))
                    {
                        text.AddSentence(sentence);
                        sentence = new Sentence();
                    }
                }
            }
        }
        if (wordBuilder.Length > 0)
            sentence.AddToken(new Word(wordBuilder.ToString()));

        if (sentence.Tokens.Count > 0)
            text.AddSentence(sentence);

        return text;
    }
}