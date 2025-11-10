using System.Xml;
using System.Xml.Serialization;

namespace ConsoleApp4
{
    [XmlInclude(typeof(Word))]
    [XmlInclude(typeof(Punctuation))]
    [XmlInclude(typeof(Sentence))]
    public abstract class Token
    {
        [XmlText]
        public string Value { get; set; }
        protected Token(string value) { Value = value; }
        public Token() { }
        public override string ToString() => Value;
    }
    [XmlRoot("Word")]
    public class Word : Token
    {
        public Word(string value) : base(value) { }
        public Word()
        {}
        public int Length => Value.Length;
    }
    [XmlRoot("Punctuation")]
    public class Punctuation : Token
    {
        public Punctuation(string value) : base(value) { }
        public Punctuation() { }
    }
    [XmlRoot("Sentence")]
    public class Sentence
    {
        [XmlElement("Word", typeof(Word))]
        [XmlElement("Punctuation", typeof(Punctuation))]
        public List<Token> Tokens { get; set; } = new List<Token>();
        public Sentence() { }

        public void AddToken(Token token) => Tokens.Add(token);

        public int WordCount => Tokens.OfType<Word>().Count();

        public override string ToString()
        {
            string result = "";
            foreach (var token in Tokens)
            {
                if (token is Word && result.Length > 0 && !result.EndsWith(" "))
                    result += " ";
                result += token.ToString();
            }
            return result;
        }
    }
    [XmlRoot("Text")]
    public class Text
    {
        [XmlElement("Sentence")]
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();

        public void AddSentence(Sentence sentence) => Sentences.Add(sentence);

        public List<Sentence> GetSentencesByWordCount()
        {
            return Sentences.OrderBy(s => s.WordCount).ToList();
        }
        public Text()
        {
           
        }

        public List<Sentence> GetSentencesByLength()
        {
            return Sentences.OrderBy(s => s.ToString().Length).ToList();
        }

        public HashSet<string> FindWordsInQuestions(int length)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var sentence in Sentences)
            {
                if (sentence.Tokens.OfType<Punctuation>().Any(p => p.Value == "?"))
                {
                    foreach (var word in sentence.Tokens.OfType<Word>())
                    {
                        if (word.Length == length)
                            result.Add(word.Value);
                    }
                }
            }
            return result;
        }

        public void RemoveWordsStartingWithConsonant(int length)
        {
            string consonants = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyz";
            foreach (var sentence in Sentences)
            {
                sentence.Tokens = sentence.Tokens
                    .Where(t => !(t is Word w && w.Length == length && consonants.Contains(w.Value[0].ToString().ToLower())))
                    .ToList();
            }
        }
        public void ReplaceWordsInSentence(int sentenceIndex, int wordLength, string replacement)
        {
            if (sentenceIndex < 0 || sentenceIndex >= Sentences.Count) return;

            var sentence = Sentences[sentenceIndex];
            for (int i = 0; i < sentence.Tokens.Count; i++)
            {
                if (sentence.Tokens[i] is Word w && w.Length == wordLength)
                    sentence.Tokens[i] = new Word(replacement);
            }
        }

        public static void RemoveStopWords(Text text, string path)
        {
            List<string> stopwords = new List<string>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    stopwords.Add(line.Trim());
                }
            }

            foreach (Sentence sentence in text.Sentences)
            {
                var newTokens = new List<Token>();

                foreach (Token token in sentence.Tokens)
                {
                    if (token is Word word)
                    {
                        if (!stopwords.Contains(word.Value, StringComparer.OrdinalIgnoreCase))
                        {
                            newTokens.Add(token);
                        }
                    }
                    else
                    {
                        newTokens.Add(token);
                    }
                }

                sentence.Tokens = newTokens;
            }
            text.Sentences.RemoveAll(s =>
                s.Tokens.All(t => t is Punctuation) ||
                s.Tokens.Count == 0);
        }

        public void ExportToXml(string filePath)
        {
            var serializer = new XmlSerializer(typeof(Text));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}