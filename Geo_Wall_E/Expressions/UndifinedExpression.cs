namespace Geo_Wall_E
{
    public class UndefinedExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.UndefinedToken;

        public Type Check(Scope scope)
        {
            return new Undefined();
        }
    }
}