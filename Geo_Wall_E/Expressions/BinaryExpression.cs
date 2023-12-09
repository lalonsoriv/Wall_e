
namespace Geo_Wall_E
{
    public class BinaryExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.BinaryExpression;
        public Expressions Left { get; private set; }
        public Token Operator { get; private set; }
        public Expressions Right { get; private set; }
        public BinaryExpression(Expressions left, Token oper, Expressions right)
        {
            Left = left;
            Operator = oper;
            Right = right;
        }

        public Type Check(Scope scope)
        {
            Type left = ((ICheckType)Left).Check(scope);
            Type right = ((ICheckType)Right).Check(scope);
            if (left is Number leftNumber && right is Number rightNumber)
            {
                switch (Operator.Type)
                {
                    case TypesOfToken.PlusToken:
                        return new Number(leftNumber.Value + rightNumber.Value);
                    case TypesOfToken.MinusToken:
                        return new Number(leftNumber.Value - rightNumber.Value);
                    case TypesOfToken.MultToken:
                        return new Number(leftNumber.Value * rightNumber.Value);
                    case TypesOfToken.PowToken:
                        return new Number(Math.Pow(leftNumber.Value, rightNumber.Value));
                    case TypesOfToken.MoreToken:
                        if (leftNumber.Value > rightNumber.Value) return new Number(1);
                        else return new Number(0);
                    case TypesOfToken.MoreOrEqualToken:
                        if (leftNumber.Value >= rightNumber.Value) return new Number(1);
                        else return new Number(0);
                    case TypesOfToken.LessToken:
                        if (leftNumber.Value < rightNumber.Value) return new Number(1);
                        else return new Number(0);
                    case TypesOfToken.LessOrEqualToken:
                        if (leftNumber.Value <= rightNumber.Value) return new Number(1);
                        else return new Number(0);
                    case TypesOfToken.DoubleEqualToken:
                        if (leftNumber.Value == rightNumber.Value) return new Number(1);
                        else return new Number(0);
                    case TypesOfToken.NoEqualToken:
                        if (leftNumber.Value != rightNumber.Value) return new Number(1);
                        else return new Number(0);
                    case TypesOfToken.DivToken:
                        if (rightNumber.Value == 0) throw new TypeCheckerError(0, 0, "No se puede dividir por cero");
                        return new Number(leftNumber.Value / rightNumber.Value);
                    case TypesOfToken.ModuToken:
                        if (rightNumber.Value == 0) throw new TypeCheckerError(0, 0, "No se puede dividir por cero");
                        return new Number(leftNumber.Value % rightNumber.Value);
                    case TypesOfToken.AndToken:
                        if (leftNumber.Value != 0 && rightNumber.Value != 0) return new Number(1);
                        else return new Number(0);
                    case TypesOfToken.OrToken:
                        if (leftNumber.Value == 1 || rightNumber.Value == 1) return new Number(1);
                        else return new Number(0);
                    default:
                        throw new TypeCheckerError(0, 0, "No es posible realizar la operación " + Operator.Lexeme);
                }
            }
            else if (left is Measure leftMeasure && right is Measure rightMeasure)
            {
                return Operator.Type switch
                {
                    TypesOfToken.PlusToken => new Measure(leftMeasure.Measure_ + rightMeasure.Measure_, ""),
                    TypesOfToken.MinusToken => new Measure(Math.Abs(leftMeasure.Measure_ - rightMeasure.Measure_), ""),
                    TypesOfToken.DivToken => new Number((int)(leftMeasure.Measure_ / rightMeasure.Measure_)),
                    TypesOfToken.MoreToken => (leftMeasure.Measure_ > rightMeasure.Measure_) ? new Number(1) : new Number(0),
                    TypesOfToken.MoreOrEqualToken => (leftMeasure.Measure_ >= rightMeasure.Measure_) ? new Number(1) : new Number(0),
                    TypesOfToken.LessOrEqualToken => (leftMeasure.Measure_ < rightMeasure.Measure_) ? new Number(1) : new Number(0),
                    TypesOfToken.LessToken => (leftMeasure.Measure_ <= rightMeasure.Measure_) ? new Number(1) : new Number(0),
                    TypesOfToken.EqualToken => (leftMeasure.Measure_ == rightMeasure.Measure_) ? new Number(1) : new Number(0),
                    TypesOfToken.NoEqualToken => (leftMeasure.Measure_ != rightMeasure.Measure_) ? new Number(1) : new Number(0),
                    _ => throw new TypeCheckerError(0, 0, "No es posible realizar la operación " + Operator.Lexeme),
                };
            }
            else if (left is Measure measureLeft && right is Number numberRight)
            {
                if (Operator.Type == TypesOfToken.MultToken) return new Measure(measureLeft.Measure_ * Math.Abs((int)numberRight.Value), "");
                else throw new TypeCheckerError(0, 0, "No es posible realizar la operación " + Operator.Lexeme);
            }
            else if (left is Number numberLeft && right is Measure measureRight)
            {
                if (Operator.Type == TypesOfToken.MultToken) return new Measure(measureRight.Measure_ * Math.Abs((int)numberLeft.Value), "");
                else throw new TypeCheckerError(0, 0, "No es posible realizar la operación " + Operator.Lexeme);
            }
            throw new TypeCheckerError(0, 0, "No es posible realizar la operación " + Operator.Lexeme);
        }
    }
}
