namespace Geo_Wall_E
{
    public class RayExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.RayToken;
        public Expressions Start { get; private set; }
        public Expressions Point2 { get; private set; }
        public RayExpression(Expressions start, Expressions point2)
        {
            Start = start;
            Point2 = point2;
        }

        public Type Check(Scope scope)
        {
            Type start = ((ICheckType)Start).Check(scope);
            Type point2 = ((ICheckType)Point2).Check(scope);
            if (start.TypeOfElement != TypeOfElement.Point) throw new TypeCheckerError(0, 0, "Se esperaba un punto pero en su lugar se obtuvo " + start.TypeOfElement);
            else if (point2.TypeOfElement != TypeOfElement.Point) throw new TypeCheckerError(0, 0, "Se esperaba un punto pero en su lugar se obtuvo " + point2.TypeOfElement);
            else return new Ray((Point)start, (Point)point2, "");
        }
    }
}
