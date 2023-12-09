namespace Geo_Wall_E
{
    public abstract class Error : Exception
    {
        abstract public void HandleException();
    }

    public class LexicalError : Error
    {
        public int Line { get; private set; }
        public int Column { get; private set; }
        public override string Message { get; }
        public LexicalError(int line, int column, string message) 
        {
            Line = line;
            Column = column;
            Message = "Error Léxico: " + message + ". Revise su entrada en la línea " + line + " columna " + column;
        }

        public override void HandleException()
        {
            throw this;
        }
    }
    public class SemanticError : Error
    {
        public int Line { get; private set; }
        public int Column { get; private set; }
        public override string Message { get; }
        public SemanticError(int line, int column, string message) 
        {
            Line = line;
            Column = column;
            Message = "Error Semántico: " + message;
        }

        public override void HandleException()
        {
            throw this;
        }
    }

    public class SyntaxError : Error
    {
        public int Line { get; private set; }
        public int Column { get; private set; }
        public override string Message { get; }
        public SyntaxError(int line, int column, string message) 
        {
            Line = line;
            Column = column;
            Message = "Error Sintáctico: " + message + ". Revise su entrada en la línea " + line + " columna " + column;
        }

        public override void HandleException()
        {
            throw this;
        }
    }

    public class TypeCheckerError : Error
    {
        public int Line { get; private set; }
        public int Column { get; private set; }
        public override string Message { get; }
        public TypeCheckerError(int line, int column, string message) 
        {
            Line = line;
            Column = column;
            Message = "Error Sintáctico: " + message;
        }

        public override void HandleException()
        {
            throw this;
        }
    }
}