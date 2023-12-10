using System.Text.RegularExpressions;

namespace Geo_Wall_E
{
    public class Parser
    {
        private Scope scope = new();
        private ColorStack colorStack = new();
        private readonly List<Token> tokens;
        private int current = 0;
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }
        private bool isTheEnd => currentToken.Type == TypesOfToken.EndFileToken;
        private Token currentToken => tokens[current];
        private Token previousToken => tokens[current - 1];
        private Token nextToken => tokens[current + 1];

        public List<Node> Parsing()
        {
            try
            {
                List<Node> nodes = Parse();
                return nodes;
            }
            catch (SyntaxError error)
            {
                error.HandleException();
                return null!;
            }
        }

        private List<Node> Parse()
        {
            List<Node> nodes = new();
            if (tokens[0].Type == TypesOfToken.ImportToken)
            {
                Advance();
                Match(TypesOfToken.String);
                //para quitar las comillas del string
                string fileName = tokens[1].Lexeme!.Replace("\"", "");
                string path = Path.GetFullPath(fileName);
                if (!Path.Exists(path)) throw new FileNotFoundException();
                string source = System.Text.Encoding.Default.GetString(File.ReadAllBytes(path));
                Lexer lexer = new(source);
                Parser parser = new(lexer.Return());
                nodes = parser.Parsing();

            }
            while (!isTheEnd)
            {
                nodes.Add(ParseFunction());
                ExpectedAndAdvance(TypesOfToken.SemicolonToken);
            }
            return nodes;
        }

        private Node ParseFunction()
        {
            if (Expected(TypesOfToken.ID))
            {
                if (nextToken.Type == TypesOfToken.OpenParenthesisToken)
                {
                    Token name = Advance();
                    if (!scope.variablesInFunction.ContainsKey(name.Lexeme!))
                    {
                        scope.Search(new FunctionStmt(name, null!, null!));
                        Advance();
                        List<Expressions> arguments = GetArguments();
                        MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
                        MatchAndAdvance(TypesOfToken.EqualToken);
                        Expressions body = ParseExpression();
                        MatchAndAdvance(TypesOfToken.SemicolonToken);
                        FunctionStmt function = new(name, arguments, body);
                        scope.Search(function);
                        return function;
                    }
                    current--;
                    return ParseFunctionCall();
                }
                return ParseId();
            }
            return ParseStmt();
        }

        private Node ParseStmt()
        {
            return currentToken.Type switch
            {
                TypesOfToken.ArcToken => ParseArc(),
                TypesOfToken.CircleToken => ParseCircle(),
                TypesOfToken.ColorToken => ParseColor(),
                TypesOfToken.CountToken => ParseCount(),
                TypesOfToken.DrawToken => ParseDraw(),
                TypesOfToken.ID => ParseId(),
                TypesOfToken.LineToken => ParseLine(),
                TypesOfToken.PointToken => ParsePoint(),
                TypesOfToken.RayToken => ParseRay(),
                TypesOfToken.RestToken => ParseId(),
                TypesOfToken.RestoreToken => ParseRestore(),
                TypesOfToken.SegmentToken => ParseSegment(),
                TypesOfToken.UnderscoreToken => ParseId(),
                _ => ParseExpression(),
            };
        }

        private Expressions ParseExpression()
        {
            return currentToken.Type switch
            {
                TypesOfToken.ArcToken => ParseArcExpression(),
                TypesOfToken.CircleToken => ParseCircleExpression(),
                //TypesOfToken.ID => ParseFunctionCall(),
                TypesOfToken.IfToken => ParseConditional(),
                TypesOfToken.IntersectToken => ParseIntersect(),
                TypesOfToken.LetToken => ParseLetInExpression(),
                TypesOfToken.LineToken => ParseLineExpression(),
                TypesOfToken.MeasureToken => ParseMeasure(),
                TypesOfToken.OpenBracketsToken => ParseSequence(),
                TypesOfToken.OpenParenthesisToken => ParseBetweenParenExpressions(),
                TypesOfToken.PointsToken => ParsePoints(),
                TypesOfToken.RandomsToken => ParseRandoms(),
                TypesOfToken.RayToken => ParseRayExpression(),
                TypesOfToken.RestToken => new VariableExpression(Advance()),
                TypesOfToken.SamplesToken => ParseSamples(),
                TypesOfToken.SegmentToken => ParseSegmentExpression(),
                _ => ParseLogical(),
            };
        }

        private Expressions ParseFunctionCall()
        {
            Token name = Advance();
            if (ExpectedAndAdvance(TypesOfToken.OpenParenthesisToken))
            {
                if (scope.variablesInFunction.ContainsKey(name.Lexeme!))
                {
                    //MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
                    List<Expressions> arguments = new();
                    if (!ExpectedAndAdvance(TypesOfToken.CloseParenthesisToken))
                    {
                        do
                        {
                            if (!ExpectedAndAdvance(TypesOfToken.CloseParenthesisToken))
                            {
                                Expressions argument = ParseExpression();
                                arguments.Add(argument);
                            }
                            else throw new SyntaxError(currentToken.Line, currentToken.Column, "Se esperaba la declaración de un argumento para la función en lugar de " + currentToken.Lexeme);
                        } while (ExpectedAndAdvance(TypesOfToken.SeparatorToken));
                    }
                    MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
                    return new FunctionCallExpression(name, arguments, null!);
                }
                throw new SyntaxError(currentToken.Line, currentToken.Column, "La función " + currentToken.Lexeme + " no ha sido declarada");
            }
            return new VariableExpression(name);
        }

        private Expressions ParseLogical()
        {
            Expressions expression = Equal();
            //Mientras sea '|' o '&' continuará el ciclo 
            while (Expected(TypesOfToken.AndToken, TypesOfToken.OrToken))
            {
                Token oper = Advance();
                Expressions right = Equal();
                expression = new BinaryExpression(expression, oper, right);
            }
            return expression;
        }

        private Expressions Equal()
        {
            Expressions expression = Compare();
            //Este contador verificará que no exista más de un operador de igualdad o desigualdad en la misma expresión
            int temp = 0;
            //Mientras sea '==' o '!=' continuará el ciclo 
            while (Expected(TypesOfToken.DoubleEqualToken, TypesOfToken.NoEqualToken))
            {
                Token oper = Advance();
                if (temp >= 1) throw new SyntaxError(currentToken.Line, currentToken.Column, "No es posible utilizar el operador " + currentToken.Lexeme + " más de una vez en una misma condición");
                Expressions right = Compare();
                expression = new BinaryExpression(expression, oper, right);
                temp++;
            }
            return expression;
        }

        private Expressions Compare()
        {
            Expressions expression = Term();
            //Este contador verificará que no exista más de un operador de comparación
            int temp = 0;
            //Mientras sea un operador de comparación continuará el ciclo 
            while (Expected(TypesOfToken.MoreToken, TypesOfToken.MoreOrEqualToken, TypesOfToken.LessToken, TypesOfToken.LessOrEqualToken))
            {
                Token oper = Advance();
                if (temp >= 1) throw new SyntaxError(currentToken.Line, currentToken.Column, "No es posible utilizar el operador " + currentToken.Lexeme + " más de una vez en una misma condición");
                Expressions right = Term();
                expression = new BinaryExpression(expression, oper, right);
                temp++;
            }
            return expression;
        }

        private Expressions Term()
        {
            Expressions expression = Factor();
            //Mientras sea '+' '-' continuará el ciclo 
            while (Expected(TypesOfToken.MinusToken, TypesOfToken.PlusToken))
            {
                Token oper = Advance();
                Expressions right = Factor();
                expression = new BinaryExpression(expression, oper, right);
            }
            return expression;
        }

        private Expressions Factor()
        {
            Expressions expression = Power();
            //Mientras sea '/' , '*' o '%' continuará el ciclo 
            while (Expected(TypesOfToken.MultToken, TypesOfToken.DivToken, TypesOfToken.ModuToken))
            {
                Token oper = Advance();
                Expressions right = Power();
                expression = new BinaryExpression(expression, oper, right);
            }
            return expression;
        }

        private Expressions Power()
        {
            Expressions expression = Unary();
            //Mientras sea '^' continuará el ciclo 
            if (Expected(TypesOfToken.PowToken))
            {
                Token oper = Advance();
                Expressions right = Power();
                expression = new BinaryExpression(expression, oper, right);
            }
            return expression;
        }

        private Expressions Unary()
        {
            //Si es '!' '-' '+' continuará 
            if (Expected(TypesOfToken.NotToken, TypesOfToken.MinusToken, TypesOfToken.PlusToken))
            {
                Token oper = Advance();
                return new UnaryExpression(oper, Unary());
            }
            return MathFunction();
        }

        private Expressions MathFunction()
        {
            //Si es un log continúa y devuelve el operador
            if (Expected(TypesOfToken.LogToken))
            {
                Token operlog = Advance();
                //Si no le sigue un '(' lanza un error, en caso contrario guarda el valor del argumento del logaritmo
                Match(TypesOfToken.OpenParenthesisToken);
                Expressions value = ParseBetweenParenExpressions();
                Expressions Base = ParseExpression();
                Expressions expression = new LogExpression(operlog, value, Base);
                return expression;
            }
            if (Expected(TypesOfToken.SinToken, TypesOfToken.CosToken, TypesOfToken.SqrtToken, TypesOfToken.ExpoToken))
            {
                Token oper = Advance();
                MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
                UnaryExpression expression = new(oper, ParseExpression());
                MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
                return expression;
            }
            return Literal();
        }

        private Expressions Literal()
        {
            switch (currentToken.Type)
            {
                case TypesOfToken.Number: Advance(); return new NumberExpression(previousToken);
                case TypesOfToken.String: Advance(); return new StringExpression(previousToken);
                case TypesOfToken.PIToken: Advance(); return new PIExpression((string)previousToken.Value!);
                case TypesOfToken.EToken: Advance(); return new EExpression((string)previousToken.Value!);
                case TypesOfToken.UndefinedToken: Advance(); return new UndefinedExpression();
                case TypesOfToken.ID: return ParseFunctionCall();
                default: throw new SyntaxError(currentToken.Line, currentToken.Column, "");
            }
        }

        private ConditionalExpression ParseConditional()
        {
            Advance();
            Expressions _if = ParseExpression();
            MatchAndAdvance(TypesOfToken.ThenToken);
            Expressions then = ParseExpression();
            MatchAndAdvance(TypesOfToken.ElseToken);
            Expressions _else = ParseExpression();
            return new ConditionalExpression(_if, then, _else);
        }

        private Expressions ParseLetInExpression()
        {
            Advance();
            //Crea una lista con las variables y sus valores
            List<Stmt> assigments = new();
            do
            {
                assigments.Add((Stmt)ParseStmt());
            } while (!Expected(TypesOfToken.InToken));
            //Si no le sigue 'in' lanza un error, en caso contrario guarda el valor de la expresión que va después del 'in'
            MatchAndAdvance(TypesOfToken.InToken);
            Expressions _in = ParseExpression();
            //MatchAndAdvance(TypesOfToken.SemicolonToken);
            return new LetInExpression(assigments, _in);
        }

        private BetweenParenExpressions ParseBetweenParenExpressions()
        {
            Advance();
            BetweenParenExpressions exp = new(ParseExpression());
            Match(TypesOfToken.CloseParenthesisToken);
            return exp;
        }

        private SequenceExpression ParseSequence()
        {
            Advance();
            if (ExpectedAndAdvance(TypesOfToken.CloseBracketsToken))
            {
                Empty empty = new();
                return new SequenceExpression(empty);
            }
            if (nextToken.Type == TypesOfToken.ThreeDotsToken)
            {
                Match(TypesOfToken.Number);
                Token start = Advance();
                Advance();
                if (Expected(TypesOfToken.Number))
                {
                    Token end = Advance();
                    MatchAndAdvance(TypesOfToken.CloseBracketsToken);
                    return new SequenceExpression(start, end);
                }
                MatchAndAdvance(TypesOfToken.CloseBracketsToken);
                return new SequenceExpression(start);
            }
            List<Expressions> sequence = new()
            {
                ParseExpression()
            };
            while (ExpectedAndAdvance(TypesOfToken.SeparatorToken))
            {
                sequence.Add(ParseExpression());
            }
            MatchAndAdvance(TypesOfToken.CloseBracketsToken);
            if (ExpectedAndAdvance(TypesOfToken.PlusToken)) return ParseConcatenatedSequence(sequence);
            return new SequenceExpression(sequence);
        }

        private SequenceExpression ParseConcatenatedSequence(List<Expressions> sequence)
        {
            SequenceExpression sequence_ = new(sequence);
            if (Expected(TypesOfToken.UndefinedToken))
            {
                Advance();
                UndefinedExpression undefined = new();
                if (ExpectedAndAdvance(TypesOfToken.PlusToken))
                {
                    Match(TypesOfToken.OpenBracketsToken);
                    return new SequenceExpression(new ConcatenatedSequenceExpression(undefined, ParseSequence()));
                }
                return new SequenceExpression(new ConcatenatedSequenceExpression(sequence_, undefined));
            }
            Match(TypesOfToken.OpenBracketsToken);
            return new SequenceExpression(new ConcatenatedSequenceExpression(sequence_, ParseSequence()));
        }

        private Samples ParseSamples()
        {
            Advance();
            MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
            MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
            return new Samples();
        }

        private Randoms ParseRandoms()
        {
            Advance();
            MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
            MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
            return new Randoms();
        }

        private RandomPoints ParsePoints()
        {
            Advance();
            MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
            Expressions figure = ParseExpression();
            MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
            return new RandomPoints(figure);
        }

        private MeasureExpression ParseMeasure()
        {
            Advance();
            MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
            Expressions point1 = ParseExpression();
            MatchAndAdvance(TypesOfToken.SeparatorToken);
            Expressions point2 = ParseExpression();
            MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
            return new MeasureExpression(point1, point2);
        }

        private Expressions ParseIntersect()
        {
            Advance();
            MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
            Expressions figure1 = ParseExpression();
            MatchAndAdvance(TypesOfToken.SeparatorToken);
            Expressions figure2 = ParseExpression();
            MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
            return new IntersectExpression(figure1, figure2);
        }

        private Node ParseDraw()
        {
            Advance();
            if (ExpectedAndAdvance(TypesOfToken.OpenBracketsToken)) return ParseDrawSequence();
            Node expression = ParseStmt();
            if (Expected(TypesOfToken.String))
            {
                Token name = Advance();
                MatchAndAdvance(TypesOfToken.SemicolonToken);
                return new DrawStmt(name.Lexeme!, expression, colorStack.Peek());
            }
            MatchAndAdvance(TypesOfToken.SemicolonToken);
            return new DrawStmt("", expression, colorStack.Peek());
        }

        private Node ParseDrawSequence()
        {
            Match(TypesOfToken.ID);
            List<VariableExpression> variables = new()
            {
                new VariableExpression(Advance())
            };
            if (ExpectedAndAdvance(TypesOfToken.CloseBracketsToken)) return new DrawStmt("", variables, colorStack.Peek());
            else
            {
                do
                {
                    MatchAndAdvance(TypesOfToken.SeparatorToken);
                    Match(TypesOfToken.ID);
                    variables.Add(new VariableExpression(Advance()));

                } while (!ExpectedAndAdvance(TypesOfToken.CloseBracketsToken));
                if (Expected(TypesOfToken.String))
                {
                    Token name = Advance();
                    MatchAndAdvance(TypesOfToken.SemicolonToken);
                    return new DrawStmt(name.Lexeme!, variables, colorStack.Peek());
                }
                MatchAndAdvance(TypesOfToken.SemicolonToken);
                return new DrawStmt("", variables, colorStack.Peek());
            }
        }

        private Count ParseCount()
        {
            Advance();
            MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
            Expressions sequence = ParseExpression();
            MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
            return new Count(sequence);
        }

        private Node ParseSegment()
        {
            if (nextToken.Type == TypesOfToken.OpenParenthesisToken) return ParseSegmentExpression();
            Advance();
            if (ExpectedAndAdvance(TypesOfToken.SequenceToken))
            {
                Match(TypesOfToken.ID);
                Token id = Advance();
                MatchAndAdvance(TypesOfToken.SemicolonToken);
                return new SegmentStmt(id, true);
            }
            Match(TypesOfToken.ID);
            Token Id = Advance();
            MatchAndAdvance(TypesOfToken.SemicolonToken);
            return new SegmentStmt(Id, false);
        }

        private SegmentExpression ParseSegmentExpression()
        {
            Advance();
            MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
            Expressions start = ParseExpression();
            MatchAndAdvance(TypesOfToken.SeparatorToken);
            Expressions end = ParseExpression();
            MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
            return new SegmentExpression(start, end);
        }

        private Node ParseRay()
        {
            if (nextToken.Type == TypesOfToken.OpenParenthesisToken) return ParseRayExpression();
            Advance();
            if (ExpectedAndAdvance(TypesOfToken.SequenceToken))
            {
                Match(TypesOfToken.ID);
                Token id = Advance();
                MatchAndAdvance(TypesOfToken.SemicolonToken);
                return new RayStmt(id, true);
            }
            Match(TypesOfToken.ID);
            Token Id = Advance();
            MatchAndAdvance(TypesOfToken.SemicolonToken);
            return new RayStmt(Id, false);
        }

        private RayExpression ParseRayExpression()
        {
            Advance();
            MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
            Expressions start = ParseExpression();
            MatchAndAdvance(TypesOfToken.SeparatorToken);
            Expressions point2 = ParseExpression();
            MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
            return new RayExpression(start, point2);
        }

        private Node ParsePoint()
        {
            Advance();
            if (ExpectedAndAdvance(TypesOfToken.SequenceToken))
            {
                Match(TypesOfToken.ID);
                Token id = Advance();
                MatchAndAdvance(TypesOfToken.SemicolonToken);
                return new PointStmt(id, true);
            }
            Match(TypesOfToken.ID);
            Token Id = Advance();
            MatchAndAdvance(TypesOfToken.SemicolonToken);
            return new PointStmt(Id, false);
        }

        private Node ParseLine()
        {
            if (nextToken.Type == TypesOfToken.OpenParenthesisToken) return ParseLineExpression();
            Advance();
            if (ExpectedAndAdvance(TypesOfToken.SequenceToken))
            {
                Match(TypesOfToken.ID);
                Token id = Advance();
                MatchAndAdvance(TypesOfToken.SemicolonToken);
                return new LineStmt(id, true);
            }
            Match(TypesOfToken.ID);
            Token Id = Advance();
            MatchAndAdvance(TypesOfToken.SemicolonToken);
            return new LineStmt(Id, false);

        }

        private LineExpression ParseLineExpression()
        {
            Advance();
            MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
            Expressions point1 = ParseExpression();
            MatchAndAdvance(TypesOfToken.SeparatorToken);
            Expressions point2 = ParseExpression();
            MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
            return new LineExpression(point1, point2);
        }

        private Node ParseCircle()
        {
            if (nextToken.Type == TypesOfToken.OpenParenthesisToken) return ParseCircleExpression();
            Advance();
            if (ExpectedAndAdvance(TypesOfToken.SequenceToken))
            {
                Match(TypesOfToken.ID);
                Token id = Advance();
                MatchAndAdvance(TypesOfToken.SemicolonToken);
                return new CircleStmt(id, true);
            }
            Match(TypesOfToken.ID);
            Token Id = Advance();
            MatchAndAdvance(TypesOfToken.SemicolonToken);
            return new CircleStmt(Id, false);
        }

        private CircleExpression ParseCircleExpression()
        {
            Advance();
            MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
            Expressions center = ParseExpression();
            MatchAndAdvance(TypesOfToken.SeparatorToken);
            Expressions measure = ParseExpression();
            MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
            return new CircleExpression(center, measure);
        }

        private Node ParseArc()
        {
            if (nextToken.Type == TypesOfToken.OpenParenthesisToken) return ParseArcExpression();
            Advance();
            if (ExpectedAndAdvance(TypesOfToken.SequenceToken))
            {
                Match(TypesOfToken.ID);
                Token id = Advance();
                MatchAndAdvance(TypesOfToken.SemicolonToken);
                return new ArcStmt(id, true);
            }
            Match(TypesOfToken.ID);
            Token Id = Advance();
            MatchAndAdvance(TypesOfToken.SemicolonToken);
            return new ArcStmt(Id, false);
        }

        private ArcExpression ParseArcExpression()
        {
            Advance();
            MatchAndAdvance(TypesOfToken.OpenParenthesisToken);
            Expressions center = ParseExpression();
            MatchAndAdvance(TypesOfToken.SeparatorToken);
            Expressions point1 = ParseExpression();
            MatchAndAdvance(TypesOfToken.SeparatorToken);
            Expressions point2 = ParseExpression();
            MatchAndAdvance(TypesOfToken.SeparatorToken);
            Expressions measure = ParseExpression();
            MatchAndAdvance(TypesOfToken.CloseParenthesisToken);
            return new ArcExpression(center, point1, point2, measure);
        }

        private Node ParseRestore()
        {
            Advance();
            colorStack.Pop();
            MatchAndAdvance(TypesOfToken.SemicolonToken);
            return new Empty();
        }

        private Node ParseColor()
        {
            Advance();
            if (Expected(TypesOfToken.ColorBlackToken)) colorStack.Push(Color.BLACK);
            else if (Expected(TypesOfToken.ColorBlueToken)) colorStack.Push(Color.BLUE);
            else if (Expected(TypesOfToken.ColorCyanToken)) colorStack.Push(Color.CYAN);
            else if (Expected(TypesOfToken.ColorGrayToken)) colorStack.Push(Color.GRAY);
            else if (Expected(TypesOfToken.ColorGreenToken)) colorStack.Push(Color.GREEN);
            else if (Expected(TypesOfToken.ColorMagentaToken)) colorStack.Push(Color.MAGENTA);
            else if (Expected(TypesOfToken.ColorRedToken)) colorStack.Push(Color.RED);
            else if (Expected(TypesOfToken.ColorWhiteToken)) colorStack.Push(Color.WHITE);
            else if (Expected(TypesOfToken.ColorYellowToken)) colorStack.Push(Color.YELLOW);
            else throw new SyntaxError(currentToken.Line, currentToken.Column, "Se esperaba la declaración de un color");
            Advance();
            MatchAndAdvance(TypesOfToken.SemicolonToken);
            return new Empty();
        }

        private Node ParseId()
        {
            if (nextToken.Type == TypesOfToken.OpenParenthesisToken) return ParseFunctionCall();
            if (nextToken.Type == TypesOfToken.SeparatorToken)
            {
                List<Token> names = new()
                {
                    Advance()
                };
                do
                {
                    MatchAndAdvance(TypesOfToken.SeparatorToken);
                    if (Expected(TypesOfToken.ID, TypesOfToken.UnderscoreToken, TypesOfToken.RestToken))
                    {
                        Token name = Advance();
                        names.Add(name);
                    }
                    else throw new SyntaxError(currentToken.Line, currentToken.Column, "Se esperaba una declaración de variable");
                } while (!ExpectedAndAdvance(TypesOfToken.EqualToken));
                Expressions body = ParseExpression();
                MatchAndAdvance(TypesOfToken.SemicolonToken);
                return new AssignationStmt(names, body);
            }
            if (ExpectedAndAdvance(TypesOfToken.UnderscoreToken)) return new Empty();
            if (Expected(TypesOfToken.RestToken)) return new SequenceExpression(Advance());
            if (current != 0 && previousToken.Type == TypesOfToken.DrawToken)
            {
                return new VariableExpression(Advance());
            }
            Token id = Advance();
            MatchAndAdvance(TypesOfToken.EqualToken);
            Expressions body_ = ParseExpression();
            MatchAndAdvance(TypesOfToken.SemicolonToken);
            return new AssignationStmt(id, body_);

        }

        #region Métodos auxiliares
        private bool MatchAndAdvance(TypesOfToken type)
        {
            if (type == currentToken.Type)
            {
                Advance();
                return true;
            }
            throw new SyntaxError(previousToken.Line, previousToken.Column, "Olvidó poner un " + type.ToString() + " después de " + previousToken.Lexeme);
        }
        private Token Advance()
        {
            current++;
            return previousToken;
        }
        private bool Match(TypesOfToken type)
        {
            if (type == currentToken.Type)
            {
                return true;
            }
            else
                throw new SyntaxError(previousToken.Line, previousToken.Column, "Olvidó poner un " + type.ToString() + " después de " + previousToken.Lexeme);
        }
        private bool Expected(params TypesOfToken[] types)
        {
            foreach (var type in types)
            {
                if (type == currentToken.Type)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ExpectedAndAdvance(params TypesOfToken[] types)
        {
            foreach (var type in types)
            {
                if (type == currentToken.Type)
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }
        private List<Expressions> GetArguments()
        {
            List<Expressions> arguments = new();
            //Si no posee argumentos igual se devuelven
            if (Expected(TypesOfToken.CloseParenthesisToken)) return arguments;

            //Mientras no exista ',' seguirá añadiendo valores siempre y cuando sea una ecuación que se pueda evaluar, parará siempre si hay un ')'
            do
            {
                if (Match(TypesOfToken.ID))
                {
                    arguments.Add(new VariableExpression(currentToken));
                    Advance();
                }
            } while (ExpectedAndAdvance(TypesOfToken.SeparatorToken));
            return arguments;
        }
        #endregion
    }
}
