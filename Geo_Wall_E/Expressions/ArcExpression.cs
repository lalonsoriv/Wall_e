
namespace Geo_Wall_E
{
    public class ArcExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.ArcToken;
        public Expressions Center { get; private set; }
        public Expressions Start { get; private set; }
        public Expressions End { get; private set; }
        public Expressions Measure { get; private set; }

        public ArcExpression(Expressions center, Expressions start, Expressions end, Expressions measure)
        {
            Center = center;
            Start = start;
            End = end;
            Measure = measure;
        }

        public Type Check(Scope scope)
        {
            Type center = ((ICheckType)Center).Check(scope);
            Type start = ((ICheckType)Start).Check(scope);
            Type end = ((ICheckType)End).Check(scope);
            Type measure = ((ICheckType)Measure).Check(scope);
            if (center.TypeOfElement != TypeOfElement.Point) throw new TypeCheckerError(0, 0, "Se esperaba un punto pero en su lugar se obtuvo " + center.TypeOfElement);
            else if (start.TypeOfElement != TypeOfElement.Point) throw new TypeCheckerError(0, 0, "Se esperaba un punto pero en su lugar se obtuvo " + start.TypeOfElement);
            else if (end.TypeOfElement != TypeOfElement.Point) throw new TypeCheckerError(0, 0, "Se esperaba un punto pero en su lugar se obtuvo " + end.TypeOfElement);
            else if (measure.TypeOfElement != TypeOfElement.Measure) throw new TypeCheckerError(0, 0, "Se esperaba una medida pero en su lugar se obtuvo " + measure.TypeOfElement);
            else return new Arc((Point)center, (Point)start, (Point)end, (Measure)measure, null!);
        }
    }
}
