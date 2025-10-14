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
    }
}
