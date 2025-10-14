using System.Text.RegularExpressions;

namespace ConsoleApp4
{
    public static class TextParser
    {
        public static Text Parse(string input)
        {
            Text text = new Text();
            string[] sentenceStrings = Regex.Split(input, @"(?<=[\.!\?])\s+");
            foreach (var s in sentenceStrings)
            {
                if (string.IsNullOrWhiteSpace(s)) continue;
                Sentence sentence = new Sentence();
                foreach (Match match in Regex.Matches(s, @"\w+|[^\w\s]"))
                {
                    string tokenStr = match.Value;
                    if (Regex.IsMatch(tokenStr, @"\w+"))
                        sentence.AddToken(new Word(tokenStr));
                    else
                        sentence.AddToken(new Punctuation(tokenStr));
                }

                text.AddSentence(sentence);
            }
            return text;
        }
    }
}
