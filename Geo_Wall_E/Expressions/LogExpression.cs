namespace Geo_Wall_E
{
    public class LogExpression : Expressions, ICheckType
    {
        public Token Log { get; private set; }
        public Expressions Value { get; private set; }
        public Expressions Base { get; private set; }

        public override TypesOfToken Type => TypesOfToken.LogToken;

        public LogExpression(Token log, Expressions value, Expressions base_)
        {
            Log = log;
            Value = value;
            Base = base_;
        }

        public Type Check(Scope scope)
        {
            Type value = ((ICheckType)Value).Check(scope);
            Type base_ = ((ICheckType)Base).Check(scope);
            if (value.TypeOfElement != TypeOfElement.Number) throw new TypeCheckerError(0, 0, "Se esperaba un número pero en su lugar se obtuvo " + value.TypeOfElement);
            if (base_.TypeOfElement != TypeOfElement.Number) throw new TypeCheckerError(0, 0, "Se esperaba un número pero en su lugar se obtuvo " + base_.TypeOfElement);
            else return new Number(Math.Log(((Number)value).Value, ((Number)base_).Value));
        }
    }
}
