
namespace Geo_Wall_E
{
    public class ConditionalExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.ConditionalExpression;
        public Expressions If { get; private set; }
        public Expressions Then { get; private set; }
        public Expressions Else { get; private set; }
        public ConditionalExpression(Expressions ifExp, Expressions thenExp, Expressions elseExp)
        {
            If = ifExp;
            Then = thenExp;
            Else = elseExp;
        }

        public Type Check(Scope scope)
        {
            Type if_ = ((ICheckType)If).Check(scope);
            {
                if (if_.TypeOfElement == TypeOfElement.Number)
                {
                    if (((Number)if_).Value == 0)
                    {
                        return ((ICheckType)Else).Check(scope);
                    }
                    else return ((ICheckType)Then).Check(scope);
                }
                if (if_.TypeOfElement == TypeOfElement.Undefined) return ((ICheckType)Else).Check(scope);
                if (if_.TypeOfElement == TypeOfElement.Sequence)
                {
                    if (((Sequence)if_).Elements.Count == 0) return ((ICheckType)Else).Check(scope);
                    else return ((ICheckType)Then).Check(scope);
                }
                return ((ICheckType)Then).Check(scope);
            }
        }
    }
}
