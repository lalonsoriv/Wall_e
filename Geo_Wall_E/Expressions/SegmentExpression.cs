namespace Geo_Wall_E
{
    public class SegmentExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.SegmentToken;
        public Expressions Start { get; private set; }
        public Expressions End { get; private set; }
        public SegmentExpression(Expressions start, Expressions end)
        {
            Start = start;
            End = end;
        }

        public Type Check(Scope scope)
        {
            Type start = ((ICheckType)Start).Check(scope);
            Type end = ((ICheckType)End).Check(scope);
            if (start.TypeOfElement != TypeOfElement.Point) throw new TypeCheckerError(0, 0, "Se esperaba un punto pero en su lugar se obtuvo " + start.TypeOfElement);
            else if (end.TypeOfElement != TypeOfElement.Point) throw new TypeCheckerError(0, 0, "Se esperaba un punto pero en su lugar se obtuvo " + end.TypeOfElement);
            else return new Segment((Point)start, (Point)end, "");
        }
    }
}