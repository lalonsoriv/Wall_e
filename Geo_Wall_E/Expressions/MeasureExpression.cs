namespace Geo_Wall_E
{
    public class MeasureExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.MeasureToken;
        public Expressions Point1 { get; private set; }
        public Expressions Point2 { get; private set; }

        public MeasureExpression(Expressions point1, Expressions point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public Type Check(Scope scope)
        {
            Type point1 = ((ICheckType)Point1).Check(scope);
            Type point2 = ((ICheckType)Point2).Check(scope);
            if (point1.TypeOfElement != TypeOfElement.Point) throw new TypeCheckerError(0, 0, "Se esperaba un punto pero en su lugar se obtuvo " + point1.TypeOfElement);
            else if (point2.TypeOfElement != TypeOfElement.Point) throw new TypeCheckerError(0, 0, "Se esperaba un punto pero en su lugar se obtuvo " + point2.TypeOfElement);
            else return new Measure((Point)point1, (Point)point2, "");
        }
    }
}