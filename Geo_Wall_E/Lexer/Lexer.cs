namespace Geo_Wall_E
{
    public class Lexer
    {
        //El codigo a escanear
        private readonly string input;
        //Caracter a escanear
        private int current = 0;
        //Linea actual
        private int line = 1;
        //Columna actual
        private int column = 1;
        //Primer caracter del token que se esta evaluando
        private int start = 0;
        private bool isTheEnd => current >= input.Length;

        public Lexer(string input)
        {
            this.input = input;
        }
        private List<Token> tokens = new();

        private static readonly Dictionary<string, TypesOfToken> keywords = new Dictionary<string, TypesOfToken>()
        {
            { "point",TypesOfToken.PointToken },
            { "points",TypesOfToken.PointsToken },
            { "draw",TypesOfToken.DrawToken },
            { "undefined", TypesOfToken.UndefinedToken },
            { "sequence", TypesOfToken.SequenceToken },
            { "line", TypesOfToken.LineToken },
            { "segment", TypesOfToken.SegmentToken },
            { "ray", TypesOfToken.RayToken },
            { "circle", TypesOfToken.CircleToken },
            { "restore", TypesOfToken.RestoreToken },
            { "import", TypesOfToken.ImportToken },
            { "arc", TypesOfToken.ArcToken },
            { "measure", TypesOfToken.MeasureToken },
            { "intersect", TypesOfToken.IntersectToken },
            { "count", TypesOfToken.CountToken },
            { "randoms", TypesOfToken.RandomsToken },
            { "rest", TypesOfToken.RestToken },
            { "samples", TypesOfToken.SamplesToken },
            { "if",TypesOfToken.IfToken },
            { "then",TypesOfToken.ThenToken },
            { "else",TypesOfToken.ElseToken },
            { "sin",TypesOfToken.SinToken},
            { "cos",TypesOfToken.CosToken},
            { "let", TypesOfToken.LetToken },
            { "in", TypesOfToken.InToken },
            { "log", TypesOfToken.LogToken },
            { "sqrt", TypesOfToken.SqrtToken },
            { "expo", TypesOfToken.ExpoToken },
            { "PI", TypesOfToken.PIToken },
            { "E", TypesOfToken.EToken },
            { "color",TypesOfToken.ColorToken },
            { "black",TypesOfToken.ColorBlackToken },
            { "blue",TypesOfToken.ColorBlueToken },
            { "cyan",TypesOfToken.ColorCyanToken },
            { "gray",TypesOfToken.ColorGrayToken },
            { "green",TypesOfToken.ColorGreenToken },
            { "magenta",TypesOfToken.ColorMagentaToken },
            { "red",TypesOfToken.ColorRedToken },
            { "white",TypesOfToken.ColorWhiteToken },
            { "yellow",TypesOfToken.ColorYellowToken },
        };

        private static readonly Dictionary<char, TypesOfToken> charTokens = new()
        {
            { ';', TypesOfToken.SemicolonToken },
            { '(', TypesOfToken.OpenParenthesisToken },
            { ')', TypesOfToken.CloseParenthesisToken},
            { '{', TypesOfToken.OpenBracketsToken },
            { '}', TypesOfToken.CloseBracketsToken },
            { ',', TypesOfToken.SeparatorToken },
            { '+', TypesOfToken.PlusToken },
            { '-', TypesOfToken.MinusToken },
            { '^', TypesOfToken.PowToken },
            { '/', TypesOfToken.DivToken },
            { '=', TypesOfToken.EqualToken },
            { '*', TypesOfToken.MultToken },
            { '%', TypesOfToken.ModuToken },
            { '!', TypesOfToken.NotToken },
            { '<', TypesOfToken.LessToken },
            { '>', TypesOfToken.MoreToken },
            { '|', TypesOfToken.OrToken },
            { '&', TypesOfToken.AndToken },
            { '.', TypesOfToken.ThreeDotsToken },
            { '_', TypesOfToken.UnderscoreToken },
        };
        public List<Token> Return()
        {
            try
            {
                //Mientras no llegue al final de la entrada
                while (!isTheEnd)
                {
                    start = current;
                    Scan();
                }
                tokens.Add(new Token(TypesOfToken.EndFileToken, null!, line, column + 1, null!));
                List<Token> list = tokens;
                return list;
            }
            catch (LexicalError error)
            {
                error.HandleException();
                return null!;
            }
        }
        private void Scan()
        {
            char currentChar = input[current];
            //Advance();
            switch (currentChar)
            {
                case '"': GetString(); break;
                case ' ':
                case '\t':
                case '\r':
                Advance();
                    break;
                case '\n': line++; column = 0; current++; break;
                default:
                    if (IsNumber(currentChar))
                    {
                        GetNumber();
                        break;
                    }
                    else if (IsLetter(currentChar))
                    {
                        GetID();
                        break;
                    }
                    GetOperator();
                    break;
            }
        }
        private bool IsLetter(char currentChar)
        {
            return ('a' <= currentChar && currentChar <= 'z') || ('A' <= currentChar && currentChar <= 'Z');
        }
        private bool IsNumber(char currentChar)
        {
            if ('0' <= currentChar && currentChar <= '9') return true;
            return false;
        }
        private void GetOperator()
        {
            char currentChar = input[current];
            Advance();
            if (charTokens.ContainsKey(currentChar))
            {
                switch (currentChar)
                {
                    case '(':
                        AddToken(TypesOfToken.OpenParenthesisToken); break;
                    case ')':
                        AddToken(TypesOfToken.CloseParenthesisToken); break;
                    case '{':
                        AddToken(TypesOfToken.OpenBracketsToken); break;
                    case '}':
                        AddToken(TypesOfToken.CloseBracketsToken); break;
                    case ',':
                        AddToken(TypesOfToken.SeparatorToken); break;
                    case ';':
                        AddToken(TypesOfToken.SemicolonToken); break;
                    case '_':
                        AddToken(TypesOfToken.UnderscoreToken); break;
                    case '&':
                        AddToken(TypesOfToken.AndToken); break;
                    case '|':
                        AddToken(TypesOfToken.OrToken); break;
                    case '-':
                        AddToken(TypesOfToken.MinusToken); break;
                    case '^':
                        AddToken(TypesOfToken.PowToken); break;
                    case '%':
                        AddToken(TypesOfToken.ModuToken); break;
                    case '*':
                        AddToken(TypesOfToken.MultToken); break;
                    case '+':
                        AddToken(TypesOfToken.PlusToken); break;
                    case '/':
                        if (Match('/')) GetComment();
                        else AddToken(TypesOfToken.DivToken);
                        break;
                    case '=':
                        if (Match('=')) AddToken(TypesOfToken.DoubleEqualToken);
                        else AddToken(TypesOfToken.EqualToken);
                        break;
                    case '!':
                        if (Match('=')) AddToken(TypesOfToken.NoEqualToken);
                        else AddToken(TypesOfToken.NotToken);
                        break;
                    case '<':
                        if (Match('=')) AddToken(TypesOfToken.LessOrEqualToken);
                        else AddToken(TypesOfToken.LessToken);
                        break;
                    case '>':
                        if (Match('=')) AddToken(TypesOfToken.MoreOrEqualToken);
                        else AddToken(TypesOfToken.MoreToken);
                        break;
                    case '.':
                        if (Match('.') && Match('.')) AddToken(TypesOfToken.ThreeDotsToken);
                        else throw new LexicalError(line, column, "No se reconoce este caracter");
                        break;
                    default:
                        throw new LexicalError(line, column, "No se reconoce este caracter");
                }
            }
        }

        private void GetID()
        {
            string id = "";
            while (IsNumber(Peek()) || IsLetter(Peek()) || Peek() == '_')
            {
                id += Peek();
                Advance();
            }
            TypesOfToken type = new();
            try
            {
                type = keywords[id];
                AddToken(type);

            }
            catch (KeyNotFoundException)
            {
                type = TypesOfToken.ID;
                AddToken(type, id);
            }
        }

        private void GetNumber()
        {
            string number = input[current].ToString();
            bool IsDot = false;
            Advance();
            for (int i = current; i < input.Length; i++)
            {
                //Si es una letra lanza un LexicalError 
                if (char.IsLetter(input[i])) throw new LexicalError(line, column, "Después de un número no debe escribir una letra");
                //Si ya tenía un punto y se agregó otro lanza un LexicalError
                if (input[i] == '.' && IsDot == true) throw new LexicalError(line, column, "Este número ya contenia un punto");
                //Si se tiene un punto y el caracter siguiente no es un número lanza un LexicalError
                if (!char.IsDigit(input[i]) && input[i - 1] == '.') throw new LexicalError(line, column, "Déspues de '.' se esperaba un número");
                //Si es decimal se declara verdadero Is_dot
                if (input[i] == '.') IsDot = true;
                //Si no es un número o un punto sale del ciclo 
                if (!char.IsDigit(input[i]) && input[i] != '.') break;
                number += input[i];
                Advance();
            }
            AddToken(TypesOfToken.Number, double.Parse(number));
        }
        //Ignora todo lo que se encuentre dentro del comentario
        private void GetComment()
        {
            while (isTheEnd && Peek() != '\n')
            {
                Advance();
            }
        }
        private void GetString()
        {
            string str = "";
            Advance();

            while (input[current] != '"' && current < input.Length)
            {
                str += input[current];
                Advance();
                if (input[current] == '\n') line++; column = 0;
            }

            if (current == input.Length) throw new LexicalError(line, column, "Faltan comillas en su expresión");

            Advance();

            AddToken(TypesOfToken.String, str);
        }
        private char Peek()
        {
            //Garantiza que no ocurra un LexicalError al llamar a este método
            if (isTheEnd) return '\0';
            return input[current];
        }
        private bool Match(char expected)
        {
            if (isTheEnd || input[current] != expected) return false;
            Advance();
            return true;
        }
        private void AddToken(TypesOfToken type)
        {
            AddToken(type, null);
        }
        private void AddToken(TypesOfToken type, object? literal)
        {
            string lexeme = input.Substring(start, current - start);
            tokens.Add(new Token(type, literal, line, column, lexeme));
        }
        private void Advance()
        {
            current++;
            column++;
        }
    }
}
