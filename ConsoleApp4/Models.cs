namespace ConsoleApp4
{
    public abstract class Token
    {
        public string Value { get; set; }
        protected Token(string value) { Value = value; }
        public override string ToString() => Value;
    }

    public class Word : Token
    {
        public Word(string value) : base(value) { }
        public int Length => Value.Length;
    }

    public class Punctuation : Token
    {
        public Punctuation(string value) : base(value) { }
    }

    public class Sentence
    {
        public List<Token> Tokens { get; set; } = new List<Token>();

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

    public class Text
    {
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();

        public void AddSentence(Sentence sentence) => Sentences.Add(sentence);

        public override string ToString() => string.Join(" ", Sentences.Select(s => s.ToString()));

        public List<Sentence> GetSentencesByWordCount()
        {
            return Sentences.OrderBy(s => s.WordCount).ToList();
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

                foreach (Sentence s in text.Sentences)
                {
                    List<Word> wordsToRemove = new List<Word>();
                    foreach (Token t in s.Tokens)
                    {
                        if (t is Word w && stopwords.Contains(w.Value, StringComparer.OrdinalIgnoreCase))
                        {
                            wordsToRemove.Add(w);
                        }
                    }
                    foreach (Word w in wordsToRemove)
                    {
                        s.Tokens.Remove(w);
                    }
                }
            }
        }
    }