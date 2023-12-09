namespace Geo_Wall_E
{
    public class FunctionStmt : Stmt
    {
        public override TypesOfToken Type => TypesOfToken.FunctionDeclaration;
        public Token Name { get; private set; }
        public List<Expressions> Arguments { get; private set; }
        public Expressions Body { get; private set; }
        public FunctionStmt(Token name, List<Expressions> arguments, Expressions body)
        {
            Name = name;
            Arguments = arguments;
            Body = body;
        }
    }
}