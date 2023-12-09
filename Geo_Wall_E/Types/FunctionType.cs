namespace Geo_Wall_E
{
    public class Function : Type
    {
        public override TypeOfElement TypeOfElement => TypeOfElement.Function;
        public Token Name {get; private set;}
        public List<Expressions> Arguments {get; private set;}
        public Expressions Body {get; private set;}
        public Function(Token name, List<Expressions> args, Expressions body)
        {
            Name = name;
            Arguments = args;
            Body = body;
        }
    }
}