namespace Geo_Wall_E
{
    public class Compiler
    {
        private string source;
        private static List<Token> tokens = new();
        private static List<Node> nodes = new();
        public List<IDrawable> Result { get; }
        public Compiler(string source)
        {
            this.source = source;
            Result = Compile(source);
        }

        public static List<IDrawable> Compile(string source)
        {
            List<IDrawable> Result = new();
            try
            {
                Lexer lexer = new(source);
                tokens = lexer.Return();
                Parser parser = new(tokens);
                nodes = parser.Parsing();
                Interpreter interpreter = new(nodes);
                Result = interpreter.Evaluate();
                return Result;
            }
            catch (Error e)
            {
                e.HandleException();
            }
            catch (System.Exception)
            {

            }
            return null!;
        }
    }
}