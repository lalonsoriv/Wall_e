
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
            //Scope newScope = new();
            //scope.SetScope();
            Type if_ = ((ICheckType)If).Check(scope);
            // Type then = ((ICheckType)Then).Check(scope);
            // Type else_ = ((ICheckType)Else).Check(scope);
            //if (CheckThenElse(scope, Then, Else))
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
            //else throw new TypeCheckerError(0, 0, "No se pueden devolver tipos de elementos distintos dentro de una condicional");
        }
        private bool CheckThenElse(Scope scopee, Expressions then, Expressions else_)
        {
           // var dict = scopee.GetDictionary();
            Scope newScope = scopee;
            //scope.SetDictionary(dict);
            Type then_ = ((ICheckType)then).Check(newScope);
            Type _else = ((ICheckType)else_).Check(newScope);
            bool sameType;
            if (then_.TypeOfElement == _else.TypeOfElement) sameType = true;
            else sameType = false;
            return sameType;
        }
    }
}