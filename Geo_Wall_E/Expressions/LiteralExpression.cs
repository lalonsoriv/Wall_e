namespace Geo_Wall_E
{
    public class NumberExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.Number;
        public Token Number { get; private set; }
        public NumberExpression(Token number)
        {
            Number = number;
        }

        public Type Check(Scope scope)
        {
            return new Number(double.Parse(Number.Lexeme!));
        }
    }
    public class StringExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.String;
        public Token String { get; private set; }
        public StringExpression(Token string_)
        {
            String = string_;
        }

        public Type Check(Scope scope)
        {
            return new String(String.Lexeme!);
        }
    }
    public class VariableExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.VariableToken;
        public Token Name { get; private set; }
        public VariableExpression(Token name)
        {
            Name = name;
        }

        public Type Check(Scope scope)
        {
            Type name = scope.GetTypes(Name.Lexeme!);
            return name;
        }
    }
    public class PIExpression : Expressions, ICheckType
    {
        public string Value { get; private set; }

        public override TypesOfToken Type => TypesOfToken.PIToken;

        public PIExpression(string value)
        {
            Value = value;
        }

        public Type Check(Scope scope)
        {
            return new Number(Math.PI);
        }
    }
    public class EExpression : Expressions, ICheckType
    {
        public string Value { get; private set; }

        public override TypesOfToken Type => TypesOfToken.EToken;

        public EExpression(string value)
        {
            Value = value;
        }

        public Type Check(Scope scope)
        {
            return new Number(Math.E);
        }
    }
}