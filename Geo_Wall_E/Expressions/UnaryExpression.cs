namespace Geo_Wall_E
{
    public class UnaryExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.UnaryExpression;
        public Token Operator { get; private set; }
        public Expressions ToOperate { get; private set; }


        public UnaryExpression(Token oper, Expressions toOper)
        {
            Operator = oper;
            ToOperate = toOper;
        }

        public Type Check(Scope scope)
        {
            Type toOper = ((ICheckType)ToOperate).Check(scope);
            switch (toOper.TypeOfElement)
            {
                case TypeOfElement.Number:
                    if (Operator.Type == TypesOfToken.PlusToken) return new Number(((Number)toOper).Value);
                    if (Operator.Type == TypesOfToken.MinusToken) return new Number(-((Number)toOper).Value);
                    if (Operator.Type == TypesOfToken.SqrtToken) return new Number(Math.Sqrt(((Number)toOper).Value));
                    if (Operator.Type == TypesOfToken.SinToken) return new Number(Math.Sin(((Number)toOper).Value));
                    if (Operator.Type == TypesOfToken.CosToken) return new Number(Math.Cos(((Number)toOper).Value));
                    if (Operator.Type == TypesOfToken.ExpoToken) return new Number(Math.Pow(Math.E, ((Number)toOper).Value));
                    if (Operator.Type == TypesOfToken.NotToken)
                    {
                        if (((Number)toOper).Value == 0) return new Number(1);
                        else return new Number(0);
                    }
                    else throw new TypeCheckerError(0, 0, "No es posible realizar la operción");
                default:
                    throw new TypeCheckerError(0, 0, "No es posible realizar la operción");
            }
        }
    }
}